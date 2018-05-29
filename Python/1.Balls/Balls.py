#-*- coding: utf-8 -*-
"""
Gra Kulki.

Gra zaczyna się tworząc obiekt a klasy App.
Standardowo dodaję pętlę Gtk.main() aby gra była widoczna.

Zaimplementowana przez: Radosław Kolba.
Data: 09.04.2018r.
"""
import gi
import random  # losowanie ruchu komputera
# wymagamy biblioteki w wersji min 3.0
gi.require_version('Gtk', '3.0')
from gi.repository import Gtk
from gi.repository import GdkPixbuf


class App(object):
    """
    Główna klasa, tworząca grę 'Kulki'.

    Przy uruchomieniu wywoływana zostaje metoda __init__ która tworzy składowe wyżej wymienionej gry.
    """

    def __init__(self):
        """
        Metoda która tworzy obiekt z podanymi parametrami.

        Sprawdza też czy na początku gry nie ma już ułożonych prawidłowo kul.

        Wywołuje metody:
            initialize_values() - inicjalizuje początkowe wartości.
            delete_balls() - usuwa kulki tak jak określają zasady gry.
        """
        self.window = Gtk.Window()
        self.window.set_title("Kulki")
        self.window.set_default_size(650, 650)
        self.window.connect("delete-event", lambda x, y: Gtk.main_quit())

        # legenda mapy: 0 - wolne, 1 - żółty, 2 - niebieski, 3 - czerwony, 4 - zielony, 5 - fioletowy
        # w niej zostaną zapisane informacje na temat rozłożenia kulek.
        self.map = [[0 for i in range(10)] for i in range(10)]
        # zmienna która przechowuje wartość, czy wcześniej już kliknęliśmy kulkę
        self.ball_clicked = False
        # zmienna która przechowuje wartość ostatnio klikniętej kulki
        self.ball_clicked_position = []
        # zmienna która przechowuje wartość o ilości punktów w bieżącym meczu
        self.points = 0
        # zmienna która przechowuje tablicę wszystkich wyników
        self.points_table = []

        # zmienna która przechowuje tablicę wszystkich toggle buttonów na siatce
        self.toggle_buttons = []

        #-# początek inicjalizacji #-#
        main_box = Gtk.VBox()

        # składniki main_box
        points_box = Gtk.HBox()
        rg_box = Gtk.HBox()
        restart = Gtk.Button("Graj od początku")
        restart.connect("clicked", self.restart)

        # składniki points_box
        points_label_static = Gtk.Label("Liczba punktów:")
        self.points_label = Gtk.Label()

        # składniki rg_box = rank + grid _ box
        rank_box = Gtk.VBox()
        self.grid = Gtk.Grid()

        self.grid.set_size_request(570, 570)
        mode = Gtk.ResizeMode(1)
        self.grid.set_resize_mode(mode)

        # składniki rank_box
        rank_label = Gtk.Label()
        self.result_label = Gtk.Label()

        rank_label.set_markup("<b>Ranking:</b>")

        #-# początek pakowania #-#
        # pakowanie rank_box
        rank_box.pack_start(rank_label, False, False, 0)
        rank_box.pack_start(self.result_label, False, False, 0)

        # pakowanie rg_box
        rg_box.pack_start(rank_box, False, False, 5)
        rg_box.pack_start(self.grid, False, False, 5)

        # pakowanie points_box
        points_box.pack_start(points_label_static, False, False, 0)
        points_box.pack_start(self.points_label, False, False, 0)

        # pakowanie main_box
        main_box.pack_start(points_box, False, False, 3)
        main_box.pack_start(rg_box, False, False, 3)
        main_box.pack_end(restart, False, False, 3)

        # dodawanie main_box do okienka
        self.window.add(main_box)

        # zainicjowanie reszty wartości (funkcja przydatna do restartowania gry)
        self.initialize_values()
        self.delete_balls()
        self.window.show_all()

    def on_clicked(self, toggle, x, y):
        """
        Metoda określająca zachowanie programu po naciśnięciu toggle button.

        Manipuluje położeniem kulek tak jak zażyczy sobie gracz.

        Parametr toggle: przechowuje kliknięty przycisk
        Parametry 'x' i 'y' określają miejsce na mapie (koordynaty) klikniętego przycisku

        Wywołuje metody:
            rand_balls(int X) - tworzy X kulek w losowych miejscach.
            delete_balls() - usuwa kulki tak jak określają zasady gry.

        """
        if toggle.get_active():
            # zabezpiecza przed działaniem "toggled" po wykonaniu set_active(False)
            color = self.map[x][y]
            if color != 0:
                # kliknąłem na niepuste pole
                if self.ball_clicked is False:
                    # jeżeli pierwszy raz kliknąłem na kulkę
                    self.ball_clicked = True
                    self.ball_clicked_position = [x, y]
                else:
                    # gdy już wcześniej kliknąłem jakąś kulkę
                    # odznaczam stary znacznik
                    i, j = self.ball_clicked_position
                    self.toggle_buttons[i][j].set_active(False)
                    self.ball_clicked_position = [x, y]
            else:
                # kliknąłem na puste pole
                if self.ball_clicked is True:
                    #gdy wcześniej kliknąłem na jakąś kulkę
                    self.ball_clicked = False
                    i, j = self.ball_clicked_position
                    self.map[x][y] = self.map[i][j]
                    self.map[i][j] = 0
                    ball = "kulka{}.svg".format(self.map[x][y])
                    image = Gtk.Image.new_from_pixbuf(GdkPixbuf.Pixbuf.new_from_file_at_size(ball, 35, 35))
                    self.toggle_buttons[x][y].set_image(image)
                    self.toggle_buttons[i][j].get_image().clear()
                    # odznacz oba przyciski
                    self.toggle_buttons[i][j].set_active(False)
                    self.toggle_buttons[x][y].set_active(False)
                    # dodaj kulki
                    self.rand_balls(3)
                    # funkcja usuwająca kulki gdy są w rzędzie 5 lub więcej
                    self.delete_balls()
                    # dodaj punkty
                    self.points += 1
                    self.points_label.set_markup("<b>{}</b>".format(self.points))
                else:
                    toggle.set_active(False)

    def restart(self, null):
        """
        Metoda restartująca grę.

        Usuwa kulki które po losowaniu spełniały warunki ich odrzucenia.
        Zapisuje również wynik gracza w tabeli, sortuje ją i wypisuje w kolejności malejącej.

        Parametr null: formalny parametr, pozwalający na restart.connect("clicked", self.restart)

        Wywołuje metody:
            initialize_values() - inicjalizuje początkowe wartości.
            delete_balls() - usuwa kulki tak jak określają zasady gry.
        """
        self.points_table.append(self.points)
        self.points_table.sort(reverse=True)
        value = len(self.points_table)
        markup = ""
        for i in range(value):
            if i < 5:
                markup += "<b>{}.</b> {}\n".format(i + 1, self.points_table[i])
        self.result_label.set_markup(markup)
        for child in self.grid.get_children():
            self.grid.remove(child)
        self.initialize_values()
        self.delete_balls()
        self.window.show_all()

    def initialize_values(self):
        """
        Metoda inicjalizująca podstawowe wartości gry i układająca przyciski na siatce.

        Wywołuje metody:
            rand_balls(int X) - tworzy X kulek w losowych miejscach.
        """
        # legenda: 0 - wolne, 1 - yellow, 2 - blue, 3 - red, 4 - green, 5 - violet
        self.map = [[0 for i in range(10)] for i in range(10)]
        self.ball_clicked = False
        self.ball_clicked_position = []
        self.points = 0
        self.points_label.set_markup("<b>{}</b>".format(self.points))

        # układam przyciski na siatce
        self.toggle_buttons = []
        for i in range(10):
            self.toggle_buttons.append([])
            for j in range(10):
                toggle = Gtk.ToggleButton()
                # UWAGA: zdaję sobie sprawę z błędnych wymiarów!
                # Przy pomocy znanych mi technik NIE MA MOŻLIWOŚCI na stworzenie poprawie wyświetlającego przycisku
                # zgodnego z poleceniem.
                toggle.set_size_request(57, 57)
                self.toggle_buttons[i].append(toggle)
                self.grid.attach(toggle, i, j, 1, 1)
                toggle.connect("clicked", self.on_clicked, i, j)

        # Ustawianie 50 kulek losowo
        self.rand_balls(50)

    def find_free(self):
        """
        Metoda znajdująca wolne pola na mapie.

        Zwraca wolne pole, lub -1 (oznaczające że nie ma wolnego pola).
        """
        free = [(i, j) for i in range(10) for j in range(10) if self.map[i][j] == 0]
        if free:
            return random.choice(free)
        else:
            return -1

    def rand_balls(self, value):
        """
        Metoda tworząca nowe kulki w losowych miejscach.

        Parametr value: określa ile kulek ma stworzyć

        Wywołuje metody:
            find_free() - znajduje wolne miejsce na mapie.
        """
        for i in range(value):
            if self.find_free() != -1:
                i, j = self.find_free()
                color = random.randint(1, 5)
                ball = "kulka{}.svg".format(color)
                image = Gtk.Image.new_from_pixbuf(GdkPixbuf.Pixbuf.new_from_file_at_size(ball, 35, 35))
                self.map[i][j] = color
                self.toggle_buttons[i][j].set_image(image)

    def delete_balls(self):
        """
        Metoda usuwająca kulki gdy zaistnieją warunki jak w założeniach gry.

        Wywołuje metody:
            search_vertical(int X, int Y) - szukanie pionowo. X i Y określają pozycję w której zaczynają szukać.
            search_horizontal(int X, int Y) - szukanie poziomo. X i Y określają pozycję w której zaczynają szukać.
            search_positive_slope(int X, int Y) - szukanie po skosie do góry. X i Y jak wyżej.
            search_negative_slope(int X, int Y) - szukanie po skosie na dół. X i Y jak wyżej.
        """
        delete_list = []
        for i in range(10):
            for j in range(10):
                if self.map[i][j] != 0:
                    # jeżeli trafiliśmy na kulkę
                    delete_list += self.search_vertical(i, j)
                    delete_list += self.search_horizontal(i, j)
                    delete_list += self.search_negative_slope(i, j)
                    delete_list += self.search_positive_slope(i, j)
        for i in delete_list:
            self.toggle_buttons[i[0]][i[1]].get_image().clear()
            self.map[i[0]][i[1]] = 0

    def search_vertical(self, i, j):
        """
        Metoda szukająca czy istnieje conajmniej 5 kulek tego samego koloru w tej samej kolumnie obok siebie.

        Gdy tak, zwracana zostaje tablica z koordynatami kulek.
        W przeciwnym wypadku zwracana zostaje pusta tablica.
        Parametry 'i' i 'j' określają miejsce startowe algorytmu.
        """
        table = []
        for k in range(10 - j):
            # sprawdzanie w poziomie
            if self.map[i][j + k] == self.map[i][j]:
                # jeśli natrafiliśmy na kulkę tego samego koloru
                table.append((i, j + k))
            else:
                break
        if len(table) >= 5:
            return table
        else:
            return []

    def search_horizontal(self, i, j):
        """
        Metoda szukająca czy istnieje conajmniej 5 kulek tego samego koloru w tym samym rzędzie obok siebie.

        Gdy tak, zwracana zostaje tablica z koordynatami kulek.
        W przeciwnym wypadku zwracana zostaje pusta tablica.
        Parametry 'i' i 'j' określają miejsce startowe algorytmu.
        """
        table = []
        for k in range(10 - i):
            # sprawdzanie w poziomie
            if self.map[i + k][j] == self.map[i][j]:
                # jeśli natrafiliśmy na kulkę tego samego koloru
                table.append((i + k, j))
            else:
                break
        if len(table) >= 5:
            return table
        else:
            return []

    def search_positive_slope(self, i, j):
        """
        Metoda szukająca czy istnieje conajmniej 5 kulek tego samego koloru po skosie do góry obok siebie.

        Gdy tak, zwracana zostaje tablica z koordynatami kulek.
        W przeciwnym wypadku zwracana zostaje pusta tablica.
        Parametry 'i' i 'j' określają miejsce startowe algorytmu.
        """
        table = []
        for k in range(min(10 - i, 10 - j)):
            if self.map[i + k][j + k] == self.map[i][j]:
                table.append((i + k, j + k))
            else:
                break
        if len(table) >= 5:
            return table
        else:
            return []

    def search_negative_slope(self, i, j):
        """
        Metoda szukająca czy istnieje conajmniej 5 kulek tego samego koloru po skosie na dół obok siebie.

        Gdy tak, zwracana zostaje tablica z koordynatami kulek.
        W przeciwnym wypadku zwracana zostaje pusta tablica.
        Parametry 'i' i 'j' określają miejsce startowe algorytmu.
        """
        table = []
        for k in range(min(i + 1, 10 - j)):
            if self.map[i - k][j + k] == self.map[i][j]:
                table.append((i - k, j + k))
            else:
                break
        if len(table) >= 5:
            return table
        else:
            return []


if __name__ == "__main__":
    a = App()
    Gtk.main()
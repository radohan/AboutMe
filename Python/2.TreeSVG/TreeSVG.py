# -*- coding: utf-8 -*-
"""Program zwracający drzewo w formacie SVG zapisane jako "drzewo.svg".

Wyjaśnienie:
konwersatorium.py poziom_zaglebienia wysokosc_pnia kat_alpha_w_stopniach.
Brak jakiegokolwiek z argumentów skutkuje błędem uruchomienia się programu.

Opis:
Funkcja korzysta z funkcji tree, która zwraca napis w formacie svg, reprezentujący drzewo.

Zmienne:
- poziom zagłębienia (int) - jest używany do określania zagłębienia się w rekurencji.
- wysokość pnia (int) - wysokość pnia w pixelach.
- kąt alpha (int) - szerokość pierwszego rozgałęzienia w stopniach.

Proponowane wartości:
- poziom zagłębienia drzewa większy niż 9
- wysokość pnia w przedziale 130-200
- kąt pierwszych konarów w przedziale 140-170

Przykład zastosowania:
konwersatorium.py 9 160 160
konwersatorium.py 11 146 152
"""
import math
import sys
from random import randint
import gi
gi.require_version('Gtk', '3.0')
from gi.repository import Gtk

Gtk


class Branch:
    """Obiekty reprezentujące gałąź.

    Obiekty typu Branch składają się z 2 punktów (początek i koniec), koloru i szerokości gałęzi.
    Służą w programie jako część składowa drzewa.
    """
    def __init__(self, punkt1, punkt2, col, w):
        """Opisuje nowo tworzony obiekt.

        Obiekt typu Branch będzie tworzony zgodnie z regułą: punkt startowy(x, y), punkt końcowy(x, y),
        kolor i szerokość. Gdzie x i y są naturalne.
        """
        self.p1 = punkt1
        self.p2 = punkt2
        self.color = col
        self.width = w

    def __str__(self):
        """Zamiana listy branches na tekst w formacie SVG."""
        return r'<path d="M{} {} L{} {} Z" style="stroke:rgb(153, {}, 0); stroke-width:{};"/>'.format(
            self.p1[0], self.p1[1], self.p2[0], self.p2[1], self.color, self.width)


def rekur(branches, point_s, length, width, levels, color, alpha, s):
    """Funkcja rekur zostaje wywoływana rekurencyjnie do generowania obiektów Branch i zapisywania do branches.

    Wyjaśnienie:
    rekur(tabela_Branch, punkt_startowy, wysokość, szerokość, poziom_zagłębienia, kolor, kat_alpha, różnorodność) -
    zapisuje 3 kolejne rozgałęzienia w tabeli branches i wykonuje kolejne pętle aż do levels równego 0.

    Opis:
    Funkcja oblicza punkty A, B, C - które są początkami przy następnym wywołaniu rekurencyjnym funkcji.
    Po obliczeniach następuje część zapisania w branches obiektów typu Branch.
    Na końcu następuje rekurencja - z końca gałęzi wyrastają 3 nowe.
    """
    if levels == 0:
        return 0
    else:
        # point_s - punkt startowy
        # s - różnorodność. Im większa tym większa różnorodność drzewa - większa możliwość "zawijania"
        point_a = [point_s[0] - length * math.cos(alpha), point_s[1] - length * math.sin(alpha)]
        point_b = [point_s[0], point_s[1] - length * math.sin(alpha)]
        point_c = [point_s[0] * 2 - (point_s[0] - length * math.cos(alpha)), point_s[1] - length * math.sin(alpha)]
        # point_a/b/c - następne punkty służące jako punkty startowe w rekurencji
        a = 15
        # a - wartość dodawana do koloru zielonego, efekt najlepszy od a = 10 i przy poziomie rekurencji większym od 7
        z1 = randint(-1, 1) * randint(1, s) * math.pi / 360.0
        z2 = randint(-1, 1) * randint(1, s) * math.pi / 360.0
        z3 = randint(-1, 1) * randint(1, s) * math.pi / 360.0
        # z - kąty służące utworzeniu ciekawej ekspozycji drzewa
        branches += [Branch(point_s, point_a, color, width), Branch(point_s, point_b, color, width),
                     Branch(point_s, point_c, color, width)]
        rekur(branches, point_a, 2.0 / 3.0 * length, 2.0 / 3.0 * width, levels - 1, color + a, alpha + z1,
              s + randint(30, 300))
        rekur(branches, point_b, 2.0 / 3.0 * length, 2.0 / 3.0 * width, levels - 1, color + a, alpha + z2,
              s + randint(30, 300))
        rekur(branches, point_c, 2.0 / 3.0 * length, 2.0 / 3.0 * width, levels - 1, color + a, alpha + z3,
              s + randint(30, 300))


def tree(levels, length, a):
    """Funkcja tworząca rekurencyjnie drzewo z obiektow Branch.

    Wyjasnienie:
    tree(poziom_zagłębienia, wysokość_pnia, kąt_alpha_w_stopniach) - zwraca napis w formacie svg przedstawiajacy drzewo.

    Opis:
    Funkcja korzysta z funkcji rekur, następnie rezultat zapisany w branches konwertuje na napis rozpoznawalny
    przez svg. Dodaje stosowny nagłówek i stopkę, a na koniec zapisuje wynik w pliku: "drzewo.svg".
    W tej funkcji wykorzystuje klasę Branch przy utworzeniu branches.

    Zmienne:
    - poziom zagłębienia (int) - jest używany do określania zagłębienia się w rekurencji.
    - wysokość pnia (int) - wysokość pnia w pixelach.
    - kąt alpha (int) - szerokość pierwszego rozgałęzienia w stopniach

    Proponowane wartości:
    - poziom zaglębienia drzewa większy niż 9
    - wysokość pnia w przedziale 130-200
    - kąt pierwszych konarów w przedziale 140-170
    - szerokosc* pnia w przedziale 20-30
    - kolor* (nawodnienie) drzewa w przedziale 30-130
    * - możliwość edycji jedynie zmieniając kod programu

    Przykład zastosowania:
        print tree(6, 155, 150)
    lub:
        plik = open("drzewo.svg", 'w')
        plik.write(tree(9, 160, 160))
    """
    color = 102
    # color - opisuje ilosc koloru zielonego
    width = 25.0
    # width - szerokosc pnia drzewa
    s = 20
    # s - różnorodność. Im większa tym większa różnorodność drzewa - większa możliwość zawijania
    x = 500
    # x - pozycja x-owa na ekranie
    y = 500
    # y - pozycja y-owa na ekranie
    alpha = a * math.pi / 180
    # alpha - kąt w radianach
    point_s = [x, y - length]
    # point_s - punkt startowy - czubek pnia
    branches = [Branch([x, y], point_s, color, width)]
    # branches - tablica obiektów z klasy Branch
    rekur(branches, point_s, 5.0 / 6.0 * length, 5.0 / 6.0 * width, levels, color, alpha, s)
    # pierwsze wywołanie funkcji rekur. Podane argumenty są subiektywnie najlepsze by utworzyć drzewo
    result = '''<svg xmlns="http://www.w3.org/2000/svg" version="1.1">\n'''
    result += "\n".join([str(t) for t in branches])
    result += "\n</svg>"
    return result


with open("drzewo.svg", 'w') as plik:
    plik.write(tree(int(sys.argv[1]), int(sys.argv[2]), int(sys.argv[3])))
    # Subiektywne najlepsze argumenty 11, 160, 160


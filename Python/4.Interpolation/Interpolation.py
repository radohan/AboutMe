#-*- coding: utf-8 -*-
"""
Program do wizualizacji funkcji, oraz jej interpolacji (zero, nearest, lagrange).

Program jest przyjemny do modyfikowania i bawienia się z wykresami. Dzięki możliwości zmiany odstępu następnych punktów,
dokładności rysowania wykresów oraz ustalania przedziałów zmiennoprzecinkowych można sprawdzić dowolny kawałek wykresu.

Trzeba jedynie mieć na uwadze błędy maszynowe które się zdarzają przy nieodpowiednich danych.
przykładowe wartości stałych dla poprawnych wyników:
    f(x) zwraca x ** 2
    points_step = 1
    draw_step = 0.01
    min_range, max_range = 0.0, 3.0
przykładowe wartości stałych dla niepoprawnych wyników:
    f(x) zwraca x ** 2
    points_step = 0.05
    draw_step = 0.01
    min_range, max_range = 10.0, 43.0

twórca: Radosław Kolba
"""
import numpy as np
import matplotlib.pyplot as plt


def f(x):
    """
    Funkcja określająca jaka funkcja zostanie wyświetlona przez program.

    :param x: wartość na której działa funkcja
    :return: wartość funkcji w punkcie x
    """
    # Jak zmienić wyświetlaną funkcję? - wystarczy podmienić zwracaną wartość na funkcję którą chcemy.
    # np. return x * x + 2
    return x ** 2


def zero(r, points, draw_step):
    """
    Funkcja interpolacji zero.

    Funkcja przedziałowa łącząca kolejne punkty z points odcinkami poziomymi oraz ukośnymi.

    Przykład użycia:
    zero([0.00, 0.01, ..., 2.99,  3.00],[[0.0, 0.0], [0.5, 1.0], [1.5, 4.0], [2.5, 9.0], [3.0, 9.0]])
    zero(r, points)
    :param r: (od range), tablica wartości x dla których funkcja będzie przyjmowała wartość.
    :param points: tablica uporządkowanych punktów potrzebnych do obliczania wartości funkcji zero
    :param draw_step: wartość określająca dokładność rysowania wykresu
    :return: tablica wartości po interpolacji zero
    """
    result = np.array(r)
    for i in xrange(len(r)):  # iteruje po całej r - dla każdego punktu którego chcemy wartość policzyć
        for j in xrange(len(points)):  # iteruje po tablicy punktów - dla każdego punktu
            if r[i] == points[j][0]:  # sprawdź czy jesteśmy w punkcie
                result[i] = points[j][1]  # jeżeli tak - dodaj do tablicy wynikowej wartość funkcji w punkcie j
                break
            elif r[i] < points[j][0]:  # jak nie dotarliśmy jeszcze do punktu
                if r[i] == points[j][0] - draw_step:  # sprawdź czy jesteśmy 1 krok przed
                    result[i] = (points[j - 1][1] + points[j][1]) / 2.0
                    # jeżeli tak - dodaj do tablicy wynikowej wartość funkcji w połowie sumy najbliższych punktów
                else:
                    result[i] = points[j - 1][1]
                    # w przeciwnym wypadku - dodaj do tablicy wynikowej wartość funkcji w ostatnim punkcie
                break
    return result


def nearest(r, points, draw_step):
    """
    Funkcja interpolacji nearest.

    Funkcja przedziałowa łącząca kolejne punkty z points odcinkami poziomymi oraz ukośnymi.
    Różni się od interpolacji zero tym że w połowie odległości między punktami jest 'schodek' a nie w punkcie.

    Dzięki podobnemu działaniu tych funkcji - funkcja może wykorzystywać funkcję zerio(r, points).

    Przykład użycia:
    nearest([0.00, 0.01, ..., 2.99,  3.00],[[0.0, 0.0], [0.5, 1.0], [1.5, 4.0], [2.5, 9.0], [3.0, 9.0]])
    nearest(r, points)
    :param r: (od range), tablica wartości x dla których funkcja będzie przyjmowała wartość.
    :param points: tablica uporządkowanych punktów potrzebnych do obliczania wartości funkcji zero
    :param draw_step: wartość określająca dokładność rysowania wykresu
    :return: tablica wartości po interpolacji nearest
    """
    tmp = [points[0]]
    for i in range(len(points) - 1):  # dla każdego punktu (oprócz ostatniego) zrób:
        x = (points[i + 1][0] + points[i][0]) / 2.0  # ustala środek przedziału między 2 sąsiednimi punktami
        y = points[i + 1][1]  # bierze y z następnego punktu
        tmp.append([x, y])  # tworzy punkt z wyżej wyjaśnionymi parametrami x i y
    tmp.append(points[-1])  # na końcu dodaje ostatni punkt
    return zero(r, tmp, draw_step)  # zwraca wartość funkcji zero na zmodyfikowanej tablicy


def lagrange(r, points):
    """
    Funkcja interpolacji Lagrange'a.

    Funkcja używa wzoru Lagrange'a do obliczania wartości funkcji.
    Jest bardzo podatna na błędy maszynowe.

    Przykład użycia:
    lagrange([0.00, 0.01, ..., 2.99,  3.00],[[0.0, 0.0], [0.5, 1.0], [1.5, 4.0], [2.5, 9.0], [3.0, 9.0]])
    lagrange(r, points)
    :param r: (od range), tablica wartości x dla których funkcja będzie przyjmowała wartość.
    :param points: tablica uporządkowanych punktów potrzebnych do obliczania wartości funkcji zero
    :return: tablica wartości po interpolacji Lagrange
    """
    poly = np.poly1d([0])
    for i in range(len(points)):  # dla każdego punktu zrób:
        tmp1, tmp2 = 1.0, 1.0  # zmienne pomocnicze
        for j in range(len(points)):  # dla każdego punktu różnego od i zrób:
            if i != j:
                tmp1 *= points[i][0] - points[j][0]
                tmp2 *= np.poly1d([1, 0]) - np.poly1d(points[j][0])
        poly += (tmp2 / tmp1) * points[i][1]  # algorytm zgodny ze wzorem
    return poly(r)  # zwraca tablice wartości funkcji na przedziale r


def main():
    """
    Główna funkcja programu.

    Tworzy wizualizację wykresu funkcji, wraz z różnymi interpolacjami.
    """
    points_step = 1.0
    # co ile rysuje się punkt.
    # UWAGA 1: Przy niskich wartościach jest możliwość uzyskania błędnego wyniku - błędy maszynowe.
    draw_step = 0.01
    # dokładność rysowania wykresów - co tyle x wylicza wartość funkcji.
    min_range, max_range = 0.0, 9.0
    # min/max_range - ograniczają w jakim miejscu ma zostać narysowana funkcja.
    # UWAGA 1: Wielkość przedziału jest wyznaczana od pierwszego do ostatniego punktu (1 napotkany tuż za przedziałem)
    # UWAGA 2: Przy wysokich wartościach jest możliwość uzyskania błędnego wyniku - błędy maszynowe.
    points, points_x, points_y = [], [], []
    # points - tablica tablic dwuelementowych [[x1, y1]... ] przechowująca punkty.
    # points_x - tablica x dla punktów - wartości pokolei.
    # points_y - tablica x dla punktów - wartości pokolei.
    for i in np.arange(min_range, max_range + points_step, points_step):
        j = f(i)
        points_x.append(i)
        points_y.append(j)
        points.append([i, j])

    r = np.arange(points_x[0], points_x[-1] + draw_step, draw_step)
    # zamiast 'ręcznie' dodawać by ostatni punkt należał do r, points, points_x, points_y zwiększam pętlę o jeden obrót.

    plt.plot(points_x, points_y, 'bo', label="punkty", markeredgecolor="blue", markersize=8)
    plt.plot(points_x, points_y, label="linear", linewidth=1.5, color="red")
    plt.plot(r, lagrange(r, points), label="Lagrange", linewidth=1.5, color="blue")
    plt.plot(r, nearest(r, points, draw_step), label="nearest", linewidth=1.5, color="y")
    plt.plot(r, zero(r, points, draw_step), label="zero", linewidth=1.5, color="c")

    plt.xlabel("Wartosci $x$", fontsize=23)
    plt.ylabel("Interpolacja wartosci $y$", fontsize=23)

    plt.legend(loc=3)

    plt.grid(color=(0.7, 0.8, 1.0))
    plt.grid(linestyle='-')

    plt.show()


main()

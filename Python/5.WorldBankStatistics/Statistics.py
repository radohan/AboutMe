# -*- coding: utf-8 -*-
"""
Program, który na podstawie danych udostępnianych przez Bank Światowy generuje stronę HTML.

Wygenerowana strona HTML o nazwie stats.html zawiera wykresy dotyczące:
- Wielkości obszarów uprawnych w %,
- Wielkości eksportu dóbr i usług,
- Gęstości zaludnienia,
- Ilości przyjeżdżających turystów.
Dla krajów: Czechy, Niemcy, Polska, Ukraina.

Program potrzebuje do poprawnego wykonania od kilku do nawet kilkunastu minut!
"""
import matplotlib.pyplot as plt
import StringIO
import xml.etree.ElementTree as ET
from bs4 import BeautifulSoup

    # wielkość obszarów uprawnych (key="AG.LND.AGRI.ZS")
    # wielkość eksportu dóbr i usług (key="NE.EXP.GNFS.CD")
    # gęstość zaludnienia (key="EN.POP.DNST")
    # ilość przyjeżdżających turystów (key="ST.INT.ARVL")


def main():
    """
    Główna funkcja programu, tworząca wykresy dla danych z zadania.

    Po wywołaniu funkcji tworzy się plik o nazwie stats.html
    Wywołanie może zająć kilka do kilkunastu minut.
    :return: Funkcja nic nie zwraca.
    """
    # Odczytuje dane z Czech
    with open('cze_Country_en_xml_v2.xml') as response:
        xml = response.read()
        soup = BeautifulSoup(xml, "lxml")
        CZE_uprawy_tablica_rok = []
        CZE_uprawy_tablica_wartosc = []
        CZE_eksport_tablica_rok = []
        CZE_eksport_tablica_wartosc = []
        CZE_zaludnienie_tablica_rok = []
        CZE_zaludnienie_tablica_wartosc = []
        CZE_turysci_tablica_rok = []
        CZE_turysci_tablica_wartosc = []
        for uprawy in soup.find_all(key='AG.LND.AGRI.ZS'):
            CZE_uprawy_tablica_rok.append(uprawy.next_sibling.next_sibling.text)
            CZE_uprawy_tablica_wartosc.append(uprawy.next_sibling.next_sibling.next_sibling.next_sibling.text)

        for eksport in soup.find_all(key='NE.EXP.GNFS.CD'):
            CZE_eksport_tablica_rok.append(eksport.next_sibling.next_sibling.text)
            CZE_eksport_tablica_wartosc.append(eksport.next_sibling.next_sibling.next_sibling.next_sibling.text)

        for zaludnienie in soup.find_all(key='EN.POP.DNST'):
            CZE_zaludnienie_tablica_rok.append(zaludnienie.next_sibling.next_sibling.text)
            CZE_zaludnienie_tablica_wartosc.append(zaludnienie.next_sibling.next_sibling.next_sibling.next_sibling.text)

        for turysci in soup.find_all(key='ST.INT.ARVL'):
            CZE_turysci_tablica_rok.append(turysci.next_sibling.next_sibling.text)
            CZE_turysci_tablica_wartosc.append(turysci.next_sibling.next_sibling.next_sibling.next_sibling.text)

    # Odczytuje dane z Niemczech
    with open('deu_Country_en_xml_v2.xml') as response:
        xml = response.read()
        soup = BeautifulSoup(xml, "lxml")
        DEU_uprawy_tablica_rok = []
        DEU_uprawy_tablica_wartosc = []
        DEU_eksport_tablica_rok = []
        DEU_eksport_tablica_wartosc = []
        DEU_zaludnienie_tablica_rok = []
        DEU_zaludnienie_tablica_wartosc = []
        DEU_turysci_tablica_rok = []
        DEU_turysci_tablica_wartosc = []
        for uprawy in soup.find_all(key='AG.LND.AGRI.ZS'):
            DEU_uprawy_tablica_rok.append(uprawy.next_sibling.next_sibling.text)
            DEU_uprawy_tablica_wartosc.append(uprawy.next_sibling.next_sibling.next_sibling.next_sibling.text)
        for eksport in soup.find_all(key='NE.EXP.GNFS.CD'):
            DEU_eksport_tablica_rok.append(eksport.next_sibling.next_sibling.text)
            DEU_eksport_tablica_wartosc.append(eksport.next_sibling.next_sibling.next_sibling.next_sibling.text)
        for zaludnienie in soup.find_all(key='EN.POP.DNST'):
            DEU_zaludnienie_tablica_rok.append(zaludnienie.next_sibling.next_sibling.text)
            DEU_zaludnienie_tablica_wartosc.append(zaludnienie.next_sibling.next_sibling.next_sibling.next_sibling.text)
        for turysci in soup.find_all(key='ST.INT.ARVL'):
            DEU_turysci_tablica_rok.append(turysci.next_sibling.next_sibling.text)
            DEU_turysci_tablica_wartosc.append(turysci.next_sibling.next_sibling.next_sibling.next_sibling.text)

    # Odczytuje dane z Polski
    with open('pol_Country_en_xml_v2.xml') as response:
        xml = response.read()
        soup = BeautifulSoup(xml, "lxml")
        POL_uprawy_tablica_rok = []
        POL_uprawy_tablica_wartosc = []
        POL_eksport_tablica_rok = []
        POL_eksport_tablica_wartosc = []
        POL_zaludnienie_tablica_rok = []
        POL_zaludnienie_tablica_wartosc = []
        POL_turysci_tablica_rok = []
        POL_turysci_tablica_wartosc = []
        for uprawy in soup.find_all(key='AG.LND.AGRI.ZS'):
            POL_uprawy_tablica_rok.append(uprawy.next_sibling.next_sibling.text)
            POL_uprawy_tablica_wartosc.append(uprawy.next_sibling.next_sibling.next_sibling.next_sibling.text)
        for eksport in soup.find_all(key='NE.EXP.GNFS.CD'):
            POL_eksport_tablica_rok.append(eksport.next_sibling.next_sibling.text)
            POL_eksport_tablica_wartosc.append(eksport.next_sibling.next_sibling.next_sibling.next_sibling.text)
        for zaludnienie in soup.find_all(key='EN.POP.DNST'):
            POL_zaludnienie_tablica_rok.append(zaludnienie.next_sibling.next_sibling.text)
            POL_zaludnienie_tablica_wartosc.append(zaludnienie.next_sibling.next_sibling.next_sibling.next_sibling.text)
        for turysci in soup.find_all(key='ST.INT.ARVL'):
            POL_turysci_tablica_rok.append(turysci.next_sibling.next_sibling.text)
            POL_turysci_tablica_wartosc.append(turysci.next_sibling.next_sibling.next_sibling.next_sibling.text)

    # Odczytuje dane z Ukrainy
    with open('ukr_Country_en_xml_v2.xml') as response:
        xml = response.read()
        soup = BeautifulSoup(xml, "lxml")
        UKR_uprawy_tablica_rok = []
        UKR_uprawy_tablica_wartosc = []
        UKR_eksport_tablica_rok = []
        UKR_eksport_tablica_wartosc = []
        UKR_zaludnienie_tablica_rok = []
        UKR_zaludnienie_tablica_wartosc = []
        UKR_turysci_tablica_rok = []
        UKR_turysci_tablica_wartosc = []
        for uprawy in soup.find_all(key='AG.LND.AGRI.ZS'):
            UKR_uprawy_tablica_rok.append(uprawy.next_sibling.next_sibling.text)
            UKR_uprawy_tablica_wartosc.append(uprawy.next_sibling.next_sibling.next_sibling.next_sibling.text)
        for eksport in soup.find_all(key='NE.EXP.GNFS.CD'):
            UKR_eksport_tablica_rok.append(eksport.next_sibling.next_sibling.text)
            UKR_eksport_tablica_wartosc.append(eksport.next_sibling.next_sibling.next_sibling.next_sibling.text)
        for zaludnienie in soup.find_all(key='EN.POP.DNST'):
            UKR_zaludnienie_tablica_rok.append(zaludnienie.next_sibling.next_sibling.text)
            UKR_zaludnienie_tablica_wartosc.append(zaludnienie.next_sibling.next_sibling.next_sibling.next_sibling.text)
        for turysci in soup.find_all(key='ST.INT.ARVL'):
            UKR_turysci_tablica_rok.append(turysci.next_sibling.next_sibling.text)
            UKR_turysci_tablica_wartosc.append(turysci.next_sibling.next_sibling.next_sibling.next_sibling.text)

    # ---------------------------------------------------------------------------------------------------------
    #: Szablon dokumentu.
    root = ET.Element("html")
    #: Nagłowek z tytułem
    head = ET.SubElement(root, "head")
    title = ET.SubElement(head, "title")
    title.text = "Strona pierwsza"
    #: Zawartość
    body = ET.SubElement(root, "body")
    paragraph = ET.SubElement(body, "p")

    # przygotowuję wykres dla UPRAW
    # Stworzenie odpowiednich tablic wynikowych
    plt.figure()
    uprawy_tablica_rok_wynik = []
    CZE_uprawy_tablica_wartosc_wynik = []
    DEU_uprawy_tablica_wartosc_wynik = []
    POL_uprawy_tablica_wartosc_wynik = []
    UKR_uprawy_tablica_wartosc_wynik = []
    # Tablice z końcówką _wynik przetrzymują wartości jeżeli wszystkie porównywane kraje mają ją wpisaną.
    for i in range(len(CZE_uprawy_tablica_rok)):  # Nie ma różnicy czy biorę z CZE, DEU, POL czy UKR - istotny przekrój
        if CZE_uprawy_tablica_wartosc[i] != "" and DEU_uprawy_tablica_wartosc[i] != "" \
                and POL_uprawy_tablica_wartosc[i] != "" and UKR_uprawy_tablica_wartosc[i] != "":
            uprawy_tablica_rok_wynik.append(int(CZE_uprawy_tablica_rok[i]))
            CZE_uprawy_tablica_wartosc_wynik.append(float(CZE_uprawy_tablica_wartosc[i]))
            DEU_uprawy_tablica_wartosc_wynik.append(float(DEU_uprawy_tablica_wartosc[i]))
            POL_uprawy_tablica_wartosc_wynik.append(float(POL_uprawy_tablica_wartosc[i]))
            UKR_uprawy_tablica_wartosc_wynik.append(float(UKR_uprawy_tablica_wartosc[i]))
    plt.plot(uprawy_tablica_rok_wynik, CZE_uprawy_tablica_wartosc_wynik, label='Czechy', color="blue")
    plt.plot(uprawy_tablica_rok_wynik, DEU_uprawy_tablica_wartosc_wynik, label='Niemcy', color="black")
    plt.plot(uprawy_tablica_rok_wynik, POL_uprawy_tablica_wartosc_wynik, label='Polska', color="red")
    plt.plot(uprawy_tablica_rok_wynik, UKR_uprawy_tablica_wartosc_wynik, label='Ukraina', color="gold")
    plt.ylabel("Wielkosc obszarow uprawnych", fontsize=16)
    plt.legend()

    imgdata = StringIO.StringIO()  # bufor 'imitujący' obiekt pliku
    plt.savefig(imgdata, format="svg")
    svg_txt = imgdata.getvalue()  # pobieram dane z bufora
    imgdata.close()  # czyszczę bufor ("zamykam" wirtualny plik)
    svg_uprawy = ET.fromstring(svg_txt)  # wczytuje dokument, metoda fromstring zwraca korzeń , czyli element svg
    paragraph.append(svg_uprawy)
    plt.close()

    # przygotowuję wykres dla EKSPORTU
    # Stworzenie odpowiednich tablic wynikowych
    plt.figure()
    eksport_tablica_rok_wynik = []
    CZE_eksport_tablica_wartosc_wynik = []
    DEU_eksport_tablica_wartosc_wynik = []
    POL_eksport_tablica_wartosc_wynik = []
    UKR_eksport_tablica_wartosc_wynik = []
    # Tablice z końcówką _wynik przetrzymują wartości jeżeli wszystkie porównywane kraje mają ją wpisaną.
    for i in range(len(CZE_eksport_tablica_rok)):  # Nie ma różnicy czy biorę z CZE, DEU, POL czy UKR - istotny przekrój
        if CZE_eksport_tablica_wartosc[i] != "" and DEU_eksport_tablica_wartosc[i] != "" \
                and POL_eksport_tablica_wartosc[i] != "" and UKR_eksport_tablica_wartosc[i] != "":
            eksport_tablica_rok_wynik.append(int(CZE_eksport_tablica_rok[i]))
            CZE_eksport_tablica_wartosc_wynik.append(float(CZE_eksport_tablica_wartosc[i]))
            DEU_eksport_tablica_wartosc_wynik.append(float(DEU_eksport_tablica_wartosc[i]))
            POL_eksport_tablica_wartosc_wynik.append(float(POL_eksport_tablica_wartosc[i]))
            UKR_eksport_tablica_wartosc_wynik.append(float(UKR_eksport_tablica_wartosc[i]))
    plt.plot(eksport_tablica_rok_wynik, CZE_eksport_tablica_wartosc_wynik, label='Czechy', color="blue")
    plt.plot(eksport_tablica_rok_wynik, DEU_eksport_tablica_wartosc_wynik, label='Niemcy', color="black")
    plt.plot(eksport_tablica_rok_wynik, POL_eksport_tablica_wartosc_wynik, label='Polska', color="red")
    plt.plot(eksport_tablica_rok_wynik, UKR_eksport_tablica_wartosc_wynik, label='Ukraina', color="gold")
    plt.ylabel("Wielkosc eksportu dobr i uslug", fontsize=16)
    plt.legend()

    imgdata = StringIO.StringIO()  # bufor 'imitujący' obiekt pliku
    plt.savefig(imgdata, format="svg")
    svg_txt = imgdata.getvalue()  # pobieram dane z bufora
    imgdata.close()  # czyszczę bufor ("zamykam" wirtualny plik)
    svg_eksport = ET.fromstring(svg_txt)  # wczytuje dokument, metoda fromstring zwraca korzeń , czyli element svg
    paragraph.append(svg_eksport)
    plt.close()

    # przygotowuję wykres dla ZALUDNIENIA
    # Stworzenie odpowiednich tablic wynikowych
    plt.figure()
    zaludnienie_tablica_rok_wynik = []
    CZE_zaludnienie_tablica_wartosc_wynik = []
    DEU_zaludnienie_tablica_wartosc_wynik = []
    POL_zaludnienie_tablica_wartosc_wynik = []
    UKR_zaludnienie_tablica_wartosc_wynik = []
    # Tablice z końcówką _wynik przetrzymują wartości jeżeli wszystkie porównywane kraje mają ją wpisaną.
    for i in range(len(CZE_zaludnienie_tablica_rok)):  # Nie ma różnicy czy biorę z CZE, DEU, POL czy UKR
        if CZE_zaludnienie_tablica_wartosc[i] != "" and DEU_zaludnienie_tablica_wartosc[i] != "" \
                and POL_zaludnienie_tablica_wartosc[i] != "" and UKR_zaludnienie_tablica_wartosc[i] != "":
            zaludnienie_tablica_rok_wynik.append(int(CZE_zaludnienie_tablica_rok[i]))
            CZE_zaludnienie_tablica_wartosc_wynik.append(float(CZE_zaludnienie_tablica_wartosc[i]))
            DEU_zaludnienie_tablica_wartosc_wynik.append(float(DEU_zaludnienie_tablica_wartosc[i]))
            POL_zaludnienie_tablica_wartosc_wynik.append(float(POL_zaludnienie_tablica_wartosc[i]))
            UKR_zaludnienie_tablica_wartosc_wynik.append(float(UKR_zaludnienie_tablica_wartosc[i]))
    plt.plot(zaludnienie_tablica_rok_wynik, CZE_zaludnienie_tablica_wartosc_wynik, label='Czechy', color="blue")
    plt.plot(zaludnienie_tablica_rok_wynik, DEU_zaludnienie_tablica_wartosc_wynik, label='Niemcy', color="black")
    plt.plot(zaludnienie_tablica_rok_wynik, POL_zaludnienie_tablica_wartosc_wynik, label='Polska', color="red")
    plt.plot(zaludnienie_tablica_rok_wynik, UKR_zaludnienie_tablica_wartosc_wynik, label='Ukraina', color="gold")
    plt.ylabel("Gestosc zaludnienia", fontsize=16)
    plt.legend()

    imgdata = StringIO.StringIO()  # bufor 'imitujący' obiekt pliku
    plt.savefig(imgdata, format="svg")
    svg_txt = imgdata.getvalue()  # pobieram dane z bufora
    imgdata.close()  # czyszczę bufor ("zamykam" wirtualny plik)
    svg_zaludnienie = ET.fromstring(svg_txt)  # wczytuje dokument, metoda fromstring zwraca korzeń , czyli element svg
    paragraph.append(svg_zaludnienie)
    plt.close()

    # przygotowuję wykres dla TURYŚCI
    # Stworzenie odpowiednich tablic wynikowych
    plt.figure()
    turysci_tablica_rok_wynik = []
    CZE_turysci_tablica_wartosc_wynik = []
    DEU_turysci_tablica_wartosc_wynik = []
    POL_turysci_tablica_wartosc_wynik = []
    UKR_turysci_tablica_wartosc_wynik = []
    # Tablice z końcówką _wynik przetrzymują wartości jeżeli wszystkie porównywane kraje mają ją wpisaną.
    for i in range(len(CZE_turysci_tablica_rok)):  # Nie ma różnicy czy biorę z CZE, DEU, POL czy UKR - istotny przekrój
        if CZE_turysci_tablica_wartosc[i] != "" and DEU_turysci_tablica_wartosc[i] != "" \
                and POL_turysci_tablica_wartosc[i] != "" and UKR_turysci_tablica_wartosc[i] != "":
            turysci_tablica_rok_wynik.append(int(CZE_turysci_tablica_rok[i]))
            CZE_turysci_tablica_wartosc_wynik.append(float(CZE_turysci_tablica_wartosc[i]))
            DEU_turysci_tablica_wartosc_wynik.append(float(DEU_turysci_tablica_wartosc[i]))
            POL_turysci_tablica_wartosc_wynik.append(float(POL_turysci_tablica_wartosc[i]))
            UKR_turysci_tablica_wartosc_wynik.append(float(UKR_turysci_tablica_wartosc[i]))
    plt.plot(turysci_tablica_rok_wynik, CZE_turysci_tablica_wartosc_wynik, label='Czechy', color="blue")
    plt.plot(turysci_tablica_rok_wynik, DEU_turysci_tablica_wartosc_wynik, label='Niemcy', color="black")
    plt.plot(turysci_tablica_rok_wynik, POL_turysci_tablica_wartosc_wynik, label='Polska', color="red")
    plt.plot(turysci_tablica_rok_wynik, UKR_turysci_tablica_wartosc_wynik, label='Ukraina', color="gold")
    plt.ylabel("Ilosc przyjezdzajacych turystow", fontsize=16)
    plt.legend()

    imgdata = StringIO.StringIO()  # bufor 'imitujący' obiekt pliku
    plt.savefig(imgdata, format="svg")
    svg_txt = imgdata.getvalue()  # pobieram dane z bufora
    imgdata.close()  # czyszczę bufor ("zamykam" wirtualny plik)
    svg_turysci = ET.fromstring(svg_txt)  # wczytuje dokument, metoda fromstring zwraca korzeń , czyli element svg
    paragraph.append(svg_turysci)
    plt.close()

    # plik svg jest poprawnym dokumentem XML
    # zanim go przeczytamy, informujemy bibliotekę o nowej przestrzni nazw
    # do której należą niektóre z elementów pliku SVG
    # dokumentacja: http://effbot.org/zone/element-namespaces.htm
    ET.register_namespace("", "http://www.w3.org/2000/svg")
    ET.register_namespace('xlink', 'http://www.w3.org/1999/xlink')

    # zapisujemy wynik do pliku
    with open("stats.html", "w") as f:
        f.write(ET.tostring(root))

main()
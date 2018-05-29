#-*- coding: utf-8 -*-
"""
Program konwertujący stronę w formacie json na tekst w formacie rtf.

Wywołuje funkcję main.
Po uruchomieniu programu zostaje utworzony nowy plik: Radoslaw Kolba - PyRTF.rtf z wynikiem konwesjii.
"""
import PyRTF
import urllib2
import json


def json_to_rtf(json_loaded):
    """Funkcja konwertuje tekst z formatu json do rtf.

    Po podaniu parametru json_loaded funkcja modyfikuje otrzymane dane na format RTF, korzystając z biblioteki PyRTF.

    :param json_loaded: załadowany słownik zczytany ze strony.
    :return: funkcja nic nie zwraca.
    """
    # Założenia:
    # pogrubione nagłówki, nazwy sekcji, daty   - OK
    # wyśrodkowane nagłówki, nazwy sekcji       - OK
    # kursywa data                              - OK
    # tekst podzielony na paragrafy             - OK

    doc = PyRTF.Document()
    ss = doc.StyleSheet

    # Modyfikacja stylu Normal
    ns = ss.ParagraphStyles.Normal.Copy()
    ns.SetParagraphPropertySet(PyRTF.ParagraphPropertySet(alignment=4))

    # Modyfikacja stylu Heading1
    hs = ss.ParagraphStyles.Heading1.Copy()
    hs.SetParagraphPropertySet(PyRTF.ParagraphPropertySet(alignment=3))

    ss.ParagraphStyles.append(ns)
    ss.ParagraphStyles.append(hs)

    site_title = json_loaded['feed']['title']['$t'].encode('utf-8')

    for load in json_loaded['feed']['entry']:
        # Tworzenie nowej selekcji z nagłówkiem - tytułem strony
        section = PyRTF.Section()
        doc.Sections.append(section)
        section.Header.append(site_title)

        # Wpisywanie daty i godziny
        date = load['published']['$t'].encode('utf-8')[:10]
        time = load['published']['$t'].encode('utf-8')[11:16]
        date = date + " " + time
        p = PyRTF.Paragraph(ss.ParagraphStyles.Heading2)
        p.append(PyRTF.TEXT(date, bold=True, italic=True))
        section.append(p)

        # Wpisywanie tytułu
        title = load['title']['$t'].encode('utf-8')
        p = PyRTF.Paragraph(ss.ParagraphStyles.Heading1)
        p.append(PyRTF.TEXT(title, bold=True))
        section.append(p)

        # Wpisywanie zawartości
        text = load['content']['$t'].encode('utf-8')
        cleared_text = ""  # tekst bez znaczników
        i = 0
        # Czyszczenie tekstu z '<>'
        while i < len(text) - 1:
            if text[i] == '<':
                while True:
                    if i + 1 == len(text):
                        break
                    if text[i] == '>':
                        break
                    i = i + 1
                    if text[i] == 'b' and text[i + 1] == 'r':
                        cleared_text = cleared_text + " \line "
                    # Istnieje możliwość dopisania większej liczby wyjątków - jeżeli będzie wymagało tego zadanie.
            else:
                cleared_text = cleared_text + text[i]
            i = i + 1
        cleared_text = cleared_text + "\page"  # Po skończeniu paragrafu przechodzi do następnej strony.
        p = PyRTF.Paragraph(ss.ParagraphStyles.Normal)
        p.append(PyRTF.TEXT(cleared_text))
        section.append(p)

    return doc


def main():
    """Główna funkcja programu.

    Należy w niej wpisać url do strony którą chcemy przekonwertować.
    Wykorzystuje funkcję json_to_rtf która jest główną funkcją konwertowania.
    Po wykonaniu funkcji powstaje plik Radoslaw Kolba - PyRTF.rtf.
    :return: Funkcja nic nie zwraca.
    """
    render = PyRTF.Renderer()
    json_text = urllib2.urlopen("http://polska.googleblog.com/feeds/posts/default?alt=json")
    json_loaded = json.load(json_text)
    doc = json_to_rtf(json_loaded)
    with open("Radoslaw Kolba - PyRTF.rtf", 'w') as outfile:
        render.Write(doc, outfile)

main()

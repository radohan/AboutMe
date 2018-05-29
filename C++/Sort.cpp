#include <iostream>
#include <cmath>
#include <fstream>
#include <string>
using namespace std;

void insertion_sort (int *tab, int n);//przez wstawianie
void MedOf3 (int *tab, int &L, int &P);
int partycja (int *tab, int L, int P);
void intro_sort (int *tab, int n, int poziom);//szybkie i intro
void kopiec (int *tab, int i, int n);
void heap_sort (int *tab, int n);//kopcowe

//-------------------------------
void insertion_sort (int *tab, int n)
{
    int i, j;
    int t;
    for (i=1; i<n; ++i)
    {
        t=tab[i];
        for (j=i; j>0 && t<tab[j-1]; --j)
        {
            tab[j]=tab[j-1];
        }
        tab[j]=t;
    }
}
//-------------------------------
void MedOf3 (int *tab, int &L, int &P)
{
    if (tab[++L-1]>tab[--P])
    {
        swap(tab[L-1],tab[P]);
    }
    if (tab[L-1]>tab[P/2])
    {
        swap(tab[L-1],tab[P/2]);
    }
    if (tab[P/2]>tab[P])
    {
        swap(tab[P/2],tab[P]);
    }
    swap(tab[P/2],tab[P-1]);
}
int partycja (int *tab, int L, int P)
{
    int i, j;
    if (P>=3)
    {
        MedOf3(tab,L,P);
    }
    for (i=L, j=P-2; ; )
    {
        for (; tab[i]<tab[P-1]; ++i);
        for (; j>=L && tab[j]>tab[P-1]; --j);
        if (i<j)
        {
            swap(tab[i++],tab[j--]);
        }
        else break;
    }
    swap(tab[i],tab[P-1]);
    return i;
}
void intro_sort (int *tab, int n, int poziom)
{
    int i;
    if (poziom<=0)
    {
        heap_sort(tab,n);
        return;
    }
    i=partycja(tab,0,n);
    if (i>9)
    {
        intro_sort(tab,i,poziom-1);
    }
    if (n-i>10)
    {
        intro_sort(tab+i+1,n-1-i,poziom-1);
    }
}
//-------------------------------
void kopiec (int *tab, int i, int n)
{
    int j;
    while (i<=n/2)
    {
        j=i*2;
        if (j+1<=n && tab[j+1]>tab[j])
        {
            j=j+1;
        }
        if (tab[i]<tab[j])
        {
            swap(tab[i],tab[j]);
        }
        else break;
        i=j;
    }
}
void heap_sort (int *tab, int n)
{
    int i;
    for (i=n/2; i>0; --i)
    {
        kopiec(tab-1,i,n);
    }
    for (i=n-1; i>0; --i)
    {
        swap(tab[0],tab[i]);
        kopiec(tab-1,1,i);
    }
}

//-------------------------------

int main()
{
    int *tab, n=-1,t;
    string lokalizacja;
    cout<<"Podaj nazwe pliku, bez jego rozszerzenia (domyslnie .txt): ";
    cin>>lokalizacja;
    ifstream test((lokalizacja+".txt").c_str());
    if(test.good()==false)
    {
        cout<<"Nie znaleziono takiego pliku!\n";
        return 0;
    }
    while(test)
    {
        test>>t;
        n++;
    }
    test.close();
    tab = new int [n];
    ifstream wczytaj((lokalizacja+".txt").c_str());
    for(int i=0;i<n;i++)
    {
        wczytaj>>tab[i];
    }
    intro_sort(tab,n,(int)floor(2*log(n)/M_LN2));
    insertion_sort(tab,n);

    ofstream zapisz((lokalizacja+".out"+".txt").c_str());
    if(zapisz.good()==false)
    {
        cout<<"Zapisanie nie powiodlo sie!\n";
        return 0;
    }
    for(int i=0;i<n;i++)
    {
        zapisz<<tab[i]<<" ";
    }
    cout<<"Ukonczono!\n";
    return 0;
}

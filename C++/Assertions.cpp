#include <cassert>
#include <iostream>
using namespace std;

bool F1(int tab[], int max_el, int n)
{
    for (int i=0; i<n; i++)
    {
        if (max_el<tab[i]) return 0;
    }
    return 1;
}

bool F2(int tab[], int max_el, int n)
{
    for (int i=0; i<n; i++)
    {
        if (max_el==tab[i]) return 1;
    }
    return 0;
}

int main(){
    int tab[1000];
    int i,n;
    int max_el;
    cin >> n;
    for (i=0; i<n; ++i){
        cin >> tab[i];
    }
    //pocz¹tek!
    assert(n>0);

    assert(F1(tab,tab[0],1) && F2(tab,tab[0],n));
    max_el = tab[0];


    assert(F1(tab,max_el,1) && F2(tab,max_el,n));
    i = 1;

    //N
    assert(F1(tab,max_el,i) && F2(tab,max_el,n));
    while (i < n){
        //N ^ B
        assert(F1(tab,max_el,i) && F2(tab,max_el,n) && (i<n));
        //Phi
        assert(F1(tab,max_el,i) && F2(tab,max_el,n) && (i<n));
        if (tab[i] > max_el){
            //Phi ^ C
            assert(F1(tab,max_el,i) && F2(tab,max_el,n) && (i<n) && tab[i] > max_el);
            assert((F1(tab,max_el,i) && tab[i] > max_el) && i<n));
            assert(F1(tab,tab[i],i) && tab[i]>=tab[i] && F2(tab,tab[i],n));
            assert(F1(tab,tab[i],i+1) && F2(tab,tab[i],n));
            max_el = tab[i];
            //Psi
            assert(F1(tab,max_el,i+1) && F2(tab,max_el,n));
        }
        //Phi ^ ~C
        assert(F1(tab,max_el,i) && F2(tab,max_el,n) && (i<n) && (tab[i] <= max_el));
        //Psi
        assert(F1(tab,max_el,i+1) && F2(tab,max_el,n));
        i = i+1;
        //N
        assert(F1(tab,max_el,i) && F2(tab,max_el,n));
    }
    //N ^ ~B
    assert(F1(tab,max_el,i) && F2(tab,max_el,n) && !(i<n));
    assert(F1(tab,max_el,i) && F2(tab,max_el,n) && (i>=n));
    assert(F1(tab,max_el,n) && F2(tab,max_el,n));
    cout << max_el << endl;
    return 0;
}

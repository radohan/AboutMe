#include <iostream>
#include <algorithm>
using namespace std;

void deap(int liczb,int glebia, char* temp, char* mychars)
{
    for(int i=0;i<glebia+1;i++)         //robi tylko wciecie
    {
        cout<<"\t";
    }
    if(liczb==glebia+1)                 //warunek skonczenia rekurencji
    {
        cout<<"writeln(";
        for(int z=0;z<liczb;z++)
        {
            cout<<mychars[z];
            if(z+1<liczb)cout<<",";     //wypisywanie ciagu a,b,c,..itd
        }
        cout<<")\n";
        return;
    }
    //zaczecie sie programu!!
    cout<<"if "<<mychars[glebia]<<" < "<<mychars[glebia+1]<<" then\n";//1 krok- if
    deap(liczb,glebia+1,temp,mychars);//2 krok -rekurencja
    for(int elsif=glebia;elsif<liczb-2;elsif++)//3 krok - else if
    {
        for(int i=0;i<glebia+1;i++){cout<<"\t";}//wciecie
        cout<<"else ";
        next_permutation(mychars,mychars+liczb);
        cout<<"if "<<mychars[glebia]<<" < "<<mychars[elsif+2]<<" then\n";
        deap(liczb,glebia+1,temp,mychars);
    }

    for(int i=0;i<glebia+1;i++){cout<<"\t";}//wciecie

    cout<<"else\n";//4 krok - else
    next_permutation(mychars,mychars+liczb);
    deap(liczb,glebia+1,temp,mychars);
}

int main()
{
    int programow,liczb;
    cin>>programow;
    cin>>liczb;
    for(int i=0;i<programow;i++)
    {
        char mychars[liczb];
        char temp[liczb];
        cout<<"program sort(input,output);\nvar\n";
        for(int z=0;z<=1;z++)
        {
            for(int j=0;j<liczb;j++)
            {
                char litera='a'+j;
                mychars[j]=litera;
                temp[j]=litera;
                cout<<litera;
                if(j+1<liczb)
                {
                    cout<<",";
                }
            }
            if(z==0)cout<<" : integer;\nbegin\n\treadln(";
        }
        cout<<");\n";
        deap(liczb,0,temp,mychars);
        cout<<"end.";
        if(i+1<programow)
        {
            cout<<"\n\n";
        }
    }
return 0;
}

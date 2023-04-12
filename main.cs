using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemAnalyzer
{
    class Program
    {
        static string[] ReadPoem(string fileName)
        {
            string[]  poem = File.ReadAllLines(fileName);
            return poem;
        }
        static string CompareString(string s1,string s2)
        {
            int l1 = s1.Length-1;
            int l2 = s2.Length-1;
            int count = 0;
            string w1 = s1.Substring(l1, count + 1);
            string w2 = s2.Substring(l2, count + 1);
            while (w1 == w2)
            {
                count++;
                l1--;
                l2--;
                w1 = s1.Substring(l1, count+1);
                w2 = s2.Substring(l2, count+1);
            } 
            return w1.Substring(1, w1.Length - 1);
        }
        static bool contains(string[] arr,string word)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i]==word||word=="")
                {
                    return true;
                }
            }
            return false;
        }
        int wordCount(string[] arr,string word)
        {
            int count = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i]==word)
                {
                    count++;
                }
            }
            return count;
        }
        static void AnalyzePoem(string[] poem)
        {

            string[] rhymes = new string[poem.Length];
            string[] temp = new string[poem.Length];
            int i,j;
            //Printing poem
            for ( i = 0; i < poem.Length; i++)
            {
                Console.WriteLine("{0}", poem[i]);
                for (j = 0 ; j < poem.Length; j++)
                {
                    if (i!=j)
                    {
                        string tmp = CompareString(poem[i], poem[j]);
                        if (tmp != "")
                        {
                            rhymes[i] = tmp.Length>1?tmp:"";
                            break;
                        }
                    }
                }
            }
            j = 0;
            for ( i = 0; i < rhymes.Length; i++)
            {
                //Hold unique values of rhymes at temprorary array
                if (!contains(temp, rhymes[i]))
                {
                    temp[j] = rhymes[i];
                    j++;
                }
            }
            //Then copy the values into new array which array length should be fixed
            string[] uniqueRhymes = new string[j];
            Array.Copy(temp, uniqueRhymes, j);
            bool isAdditional = true,isWord=true,isPhrase=true;
            for ( i = 0; i < rhymes.Length-1; i++)
            {
                //Check Word,Phrase and Additional rhymes
                if (rhymes[i]!=rhymes[i+1])
                {
                    isAdditional = false;
                }
                if (rhymes[i]!=rhymes[i+1]&&rhymes[i].Split(' ').Length!=2)
                {
                    isWord = false;
                }
                if (rhymes[i]!=rhymes[i+1]&&rhymes[i].Split(' ').Length<=2)
                {
                    isPhrase = false;
                }
            }
            bool isAlternating = true ,isHoarse=true,isWinding=true,isStraight=true;
            //Check Alternating and Winding rhymes
            if (rhymes.Length%4==0)
            {
                for ( i = 0; i < rhymes.Length; i += 4)
                {
                    //Check pattern and word count of Alternating rhyme
                    if (!(rhymes[i] == rhymes[i + 2] && rhymes[i + 1] == rhymes[i + 3])||rhymes[i].Split(' ').Length>1||rhymes[i+1].Split(' ').Length>1)
                    {
                        isAlternating = false;
                    }
                    if (!(rhymes[i] == rhymes[i + 2]&&rhymes[i+1]==rhymes[i+3]))
                    {
                        isWinding = false;
                    }
                }
            }
            else
            {
                isAlternating = false;
                isWinding = false;
            }
            if (rhymes.Length%3==0)
            {
                //Check Hoarse rhyme
                string tmp=null;
                for ( i = 0; i < rhymes.Length; i+=3)
                {
                    if (tmp == null)
                    {
                        tmp = rhymes[i + 1];
                        if (!(rhymes[i] == rhymes[i + 2]))
                        {
                            isHoarse = false;
                        }
                    }
                    else
                    { 
                        if (!(rhymes[i] == rhymes[i + 2] && rhymes[i] == tmp))
                        {
                            isHoarse = false;
                        }
                    }
                    if(i+3<rhymes.Length) tmp = rhymes[i + 3];
                }
            }
            else
            {
                isHoarse = false;
            }
            i = 0;
            int count = 0;
            string tmpRhyme="";
            bool check = false;
            while (i<rhymes.Length-1)
            {
                //Check straight rhyme
                if (check)
                {
                    count = 0;
                    while (rhymes[i]==tmpRhyme)
                    {
                        count--;
                        i++;
                        if (i == rhymes.Length-1)
                        {
                            count--;
                            break;
                        }
                    }
                    tmpRhyme = rhymes[i];
                    check = !check;
                }
                else
                {
                    while (rhymes[i]==tmpRhyme)
                    {
                        count++;
                        i++;
                        if (i == rhymes.Length - 1)
                        {
                            count++; 
                            break;
                        }
                    }
                    tmpRhyme = rhymes[i];
                    check = !check;
                }
            }
            if (count != 0)
            {
                isStraight = false;
            }
            //Output repetitions and rhyme type
            Console.Write("\nOutput: ");
            if (isAdditional||isWord||isPhrase)
            {
                Console.Write("Repetition: {0}\nType:", uniqueRhymes[0]);
                if (isAdditional)
                    Console.Write("Additional Rhyme");
                else if (isWord)
                    Console.Write("Word Rhyme");
                else if (isPhrase)
                    Console.Write("Phrase Rhyme");
                return;
            }
            else
            {
                Console.Write("Repetition: ");
                for (i = 0; i < uniqueRhymes.Length; i++)
                {
                    Console.Write((char)(i+65) + "- {0} ", uniqueRhymes[i]);
                }
                Console.Write("\nType: ");
                if (isAlternating)
                    Console.WriteLine("Alternating Rhyme");
                else if (isWinding)
                    Console.WriteLine("Winding Rhyme");
                else if (isHoarse)
                    Console.WriteLine("Hoarse Rhyme");
                else if (isStraight)
                    Console.WriteLine("Straight Rhyme");
                else Console.WriteLine("No rhyme detected");
            }
            
        }
        static void Main(string[] args)
        {
            try
            {
                AnalyzePoem(ReadPoem("D:\\poem.txt"));
            }
            catch (Exception)
            {
                Console.WriteLine("Error occured.");
            }
            Console.ReadKey();
        }
    }
}
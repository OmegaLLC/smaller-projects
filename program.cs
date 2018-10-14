using System;
using System.Collections.Generic;
using System.IO;

class ToDoList {
    private List<string> list;
    private string filepath = "data.txt";

    public ToDoList() {
        list = ReadFile(); //empty list if no file or empty file, else it reads previous state from txt file
    }
    
    public void Add(string todo) {
        list.Add(todo);
        WriteFile();
        Console.WriteLine("#"+(list.Count)+" "+todo);   
    }

    public int getListCount() {
        return list.Count;
    }

    public void Do(int number) {
        if (number > 0 && number <= list.Count && list[number-1] != "") {
            Console.WriteLine("Completed #"+number+" "+list[number-1]);
            list[number-1] = "";
            WriteFile();
        }
        else {
            Console.WriteLine("#"+number+" is not on the list");
        }
    }

    private List<string> ReadFile() {
        string line;
        List<string> newList = new List<string>();
        try {
            StreamReader sr = new StreamReader(filepath);
            line = sr.ReadLine();
            while (line != null) {
                newList.Add(line);
                line = sr.ReadLine();
            }
            sr.Close();
        }
        catch(Exception e) {
            Console.WriteLine("Exception: " + e.Message+". Will not load any data");
        }
        return newList;
    }

    private void WriteFile() {
        string text = "";
        foreach (string todo in list) {
            text += todo +"\n";
        }
        System.IO.File.WriteAllText(filepath, text);
    }

    public void Print() {
        for (int i = 0; i < list.Count; i++) {
            if (list[i] != "") {
                Console.WriteLine("#"+(i+1)+" "+list[i]);
            }
        }
    }
}

public class ExecuteApp {
    public static void Main(string[] args) { // try to read state from previous run saved in a file 
        ToDoList todolist = new ToDoList();
        string input;
        while (true) {
            input = Console.ReadLine();
            if (input.Length < 4) {
                Console.WriteLine("Command must be at least 4 char long");
            }
            else if(input.ToLower() == "exit") {
                break;
            }
            else if (input.Substring(0,3).ToLower() == "add" && input.Length >4) {
                string added = input.Substring(4);
                if (added[0] != '"' || added[added.Length-1] != '"' || added.Length<3) { // if it doesnt have quote end and quote start. In the assignment it showed  
                    //Console.WriteLine("Put quotation marks around what you want to add"); // "" around the strings added, but seeing as # was used weirdly too, I decided to allow both
                    todolist.Add(added);                                                    // So if theres quote marks, then they are removed, if there arent then i just add what it says
                } else {
                    added = added.Substring(0,added.Length-1); //remove quote end
                    added = added.Substring(1,added.Length-1); //remov quote start
                    todolist.Add(added); 
                }
            }
            else if (input.Substring(0,2).ToLower() == "do") { 
                string number = input.Substring(3);
                if (input[3] == '#') { // Assignment was confusing with how # is supposed to be used. task says that # is the number, but example shows usage of  Do #1, so i added both
                    if (input.Length>4) {
                        number = input.Substring(4);
                    }
                }
                int number2;
                if(int.TryParse(number, out number2)) {
                    todolist.Do(number2);
                }
                else {
                    Console.WriteLine("Not valid input for Do. Example: 'Do #2'");
                }
            }
            else if (input.Length >=5) {
                if (input.Substring(0,5).ToLower() == "print") {
                    todolist.Print();
                }
                else  {
                Console.WriteLine(input+" is not a valid command");
                }
            }
            else  {
                Console.WriteLine(input+" is not a valid command");
            }
        }
    }
}

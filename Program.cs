using System;
using System.Collections.Generic;
using System.IO;

class ToDoList {
    private List<string> list;
    private string filepath = "data.txt";

    public ToDoList() {
        list = ReadFile();
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
    public static void Main(string[] args) { /* try to read state from previous run saved in a file */
        ToDoList todolist = new ToDoList();
        string input;
        while (true) {
            input = Console.ReadLine();
            if(input == "Exit") {
                break;
            }
            else if (input.Length <5) {
                Console.WriteLine("input must be at least 5 characters"); /* minimal amount to get use any command (except exit), hinders indexing exceptions */
            }
            else if (input.Substring(0,3) == "Add") {
                string added = input.Substring(4);
                if (added[0] != '"' || added[added.Length-1] != '"' || added.Length<3) { /* if no quotation marks, dont accept. In the assignment it showed "" around what was added to the list, so I added this */
                    Console.WriteLine("Put quotation marks around what you want to add");
                } else {
                    added = added.Substring(0,added.Length-1); //remove quote end
                    added = added.Substring(1,added.Length-1); //remov quote start
                    todolist.Add(added); 
                }
            }
            else if (input.Substring(0,2) == "Do") {
                string number = input.Substring(4);
                int number2;
                if(int.TryParse(number, out number2)) {
                    todolist.Do(number2);
                }
                else {
                    Console.WriteLine("Not valid input for Do. Example: 'Do #2'");
                }
            }
            else if (input.Substring(0,5) == "Print") {
                todolist.Print();
            }
            else  {
                Console.WriteLine(input+" is not a valid command");
            }
        }
    }
}

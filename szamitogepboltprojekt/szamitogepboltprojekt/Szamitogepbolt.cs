using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace szamitogepboltprojekt
{
    internal class Szamitogepbolt
    {
        bool stopProgram = false;
        List<Component> componentList = new();
        public void fileReading(string allomanynev)
        {
            FileStream fs = new FileStream(allomanynev, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine() ?? "";
                string[] resz = line.Split(';');
                string _comp_type = resz[0];
                string _comp_name = resz[1];
                string _comp_specs = resz[2];
                double _comp_price = Convert.ToInt32(resz[3]);
                Component new_comp = new Component(_comp_type, _comp_name, _comp_specs, _comp_price);
                componentList.Add(new_comp);
            }
            sr.Close();
            fs.Close();
        }

        public void programRunning() 
        {
            while (!stopProgram)
            {
                Console.WriteLine("What would you like to do?\n[1] - Search for a component type or name\n[2] - Search between prizes\n[3] - Create statistics\n[4] - Set discounts\n[5] - Input another component\n[6] - Create a website\n[7] - Quit program");
                int a = Convert.ToInt32(Console.ReadLine());
                switch (a)
                {
                    case 1:
                        Console.WriteLine("Type the component type or name that you would want to search for: ");
                        string input = Console.ReadLine();
                        searchForComponent(input);
                        break;
                    case 2:
                        Console.WriteLine("Please type in the type range: ");
                        Console.WriteLine("Lowest price: ");
                        int price_low = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Highest price: ");
                        int price_high = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("The list of components that matches the price range: ");
                        searchFCbyprice(price_low, price_high);
                        break;
                    case 3:
                        Console.WriteLine("Creating statistics in statistics.txt");
                        createStatistics();
                        Console.WriteLine("Statistics has been created in statistics.txt");
                        break;
                    case 4:
                        Console.WriteLine("Please type in the discount in %: ");
                        int discount = Convert.ToInt32(Console.ReadLine());
                        setDiscounts(discount);
                        Console.WriteLine("The discount has been applied");
                        break; 
                    case 5:
                        Console.WriteLine("Please type in the component by the following: {Type};{Name};{Specs};{Price}");
                        string new_component = Console.ReadLine();
                        addAComponent(new_component);
                        break;
                    case 6:
                        createAWebsite();
                        Console.WriteLine("Website has been created");
                        break;
                    case 7:
                        Console.WriteLine("Quitting program");
                        quitProgram();
                        break;
                }
            }
        }

        public void searchForComponent(string input) 
        {
            int db = 0;
            foreach (Component element in componentList)
            {
                if (element.Name.ToLower() == input.ToLower() || element.Type.ToLower() == input.ToLower())
                {
                    Console.WriteLine($"Type: {element.Type}, Name: {element.Name}, Specs: {element.Specs}, Price: {element.Price}");
                    db++;
                }
            }
            if (db == 0)
            {
                Console.WriteLine("Component not found or doesn't exist in the list");
            }
        }

        public void searchFCbyprice(int price_low, int price_high) 
        { 
            int db = 0;
            foreach(Component element in componentList)
            {
                if (element.Price < price_low || element.Price > price_high)
                {
                    continue;
                }
                else 
                {
                    Console.WriteLine($"Type: {element.Type}, Name: {element.Name}, Specs: {element.Specs}, Price: {element.Price}");
                    db++;
                }
            }
            if (db == 0) 
            {
                Console.WriteLine("There is no component that matches the price range");
            }
        }

        public void createStatistics() 
        {
            int count_nvidia = 0, count_amd = 0, count_intel = 0, count_cpu = 0, count_gpu = 0, count_ssd = 0, count_hdd = 0; double avg_price = 0;
            string[] resz = { };
            foreach (Component element in componentList)
            {
                resz = element.Name.Split(" ");
                if (resz[0].ToUpper() == "NVIDIA" && element.Type.ToUpper() == "GPU")
                {
                    count_nvidia++;
                    count_gpu++;
                }
                else if (resz[0].ToUpper() == "AMD" && element.Type.ToUpper() == "GPU")
                {
                    count_amd++;
                    count_gpu++;
                }
                else if (resz[0].ToUpper() == "AMD" && element.Type.ToUpper() == "CPU")
                {
                    count_amd++;
                    count_cpu++;
                }
                else if (resz[0].ToUpper() == "INTEL" && element.Type.ToUpper() == "CPU")
                {
                    count_intel++;
                    count_cpu++;
                }
                else if (element.Type.ToUpper() == "CPU") {
                    count_cpu++;
                }
                else if (element.Type.ToUpper() == "GPU") {
                    count_gpu++;
                }
                else if (element.Type.ToUpper() == "SSD")
                {
                    count_ssd++;
                }
                else if (element.Type.ToUpper() == "HDD")
                {
                    count_hdd++;
                }
                else
                {
                    Console.WriteLine("xd");
                }
                avg_price += element.Price;
                
            }
            avg_price /= componentList.Count;
            using (StreamWriter writetext = new StreamWriter("statistics.txt")) 
            {

                writetext.WriteLine($"Average price: {avg_price}\nNvidia products in the list: {count_nvidia}\nAMD products in the list: {count_amd}\nIntel products in the list: {count_intel}\nCPUs in the list: {count_cpu}\nGPUs in the list: {count_gpu}\nSSDs in the list: {count_ssd}\nHDDs in the list: {count_hdd}\n");
            }
        }

        public void setDiscounts(int input) 
        {
            double percentage = (100 - input) / 10;
            foreach (Component element in componentList) 
            {
                element.Price = element.Price * percentage;
            }
        }

        public void addAComponent(string input) {
            string[] resz = input.Split(';');
            string _comp_type = resz[0];
            string _comp_name = resz[1];
            string _comp_specs = resz[2];
            double _comp_price = Convert.ToInt32(resz[3]);
            Component new_comp = new Component(_comp_type, _comp_name, _comp_specs, _comp_price);
            componentList.Add(new_comp);
            Console.WriteLine("Component has been added");
        }

        public void createAWebsite() {
            string veg = "";
            foreach (Component element in componentList)
            {
                veg += "<div class='card'><div class='card-body'><h4 class='card-title'>" + element.Name + "</h4><p class='card-text'><br>Type: " + element.Type + "<br>Specs: "+element.Specs+"<br>Price:" +element.Price+"</p></div></div>\n";
            }
            using (StreamWriter sw = new StreamWriter("index.html"))
            {
                sw.WriteLine("<!DOCTYPE html>\n<html lang = 'hu'>\n<head>\n<meta charset = 'UTF-8'>\n<meta name = 'viewport' content = 'width=device-width, initial-scale=1.0'>\n<title> Components website </title>\n<link href = 'https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css' rel = 'stylesheet'>\n<link rel = 'stylesheet' href = 'style.css'>\n</head>\n<body>\n<div class='container'>\n<main>\n<div class='flex-container' id='cards'>" + veg + "</div>\n</main>\n<footer>&copy; 2023</footer>\n</div>\n<script src = 'https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js' >\n</script>\n</body>\n</html>");
                sw.Close();
            }
        }

        public void quitProgram() 
        {
            using (StreamWriter sw = new StreamWriter("component_list.txt")) {
                foreach (Component element in componentList) { 
                    sw.WriteLine(element.Type + ";" + element.Name + ";" + element.Specs + ";" + element.Price);
                }
                sw.Close();
            }
            stopProgram = true;
        }
    }
}

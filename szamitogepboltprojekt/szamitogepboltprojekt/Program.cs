namespace szamitogepboltprojekt
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Szamitogepbolt szamgep = new Szamitogepbolt();
            szamgep.fileReading("component_list.txt");
            szamgep.programRunning();
        }
    }
}
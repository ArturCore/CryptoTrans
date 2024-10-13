namespace CryptoTrans.Services
{
    public class FileService
    {
        private const string FILE_PATH = "example.txt";
        private StreamWriter streamWriter;
        public FileService()
        {
            streamWriter = new StreamWriter(FILE_PATH, true); //true for adding new data to the end of file
        }

        public void WriteToFile(string text)
        {
            streamWriter.WriteLine(text);
        }

        public void OnClose()
        {
            streamWriter.Close();
        }
    }
}

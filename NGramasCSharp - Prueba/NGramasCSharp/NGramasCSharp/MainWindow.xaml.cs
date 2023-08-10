using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NGramasCSharp
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string pathCorpusFileMusic = "D:\\Prueba\\NGramasCSharp\\corpus.txt";
        string corpusFile;
        string pathCorpusFile = "D:\\Prueba\\NGramasCSharp\\corpus.txt";

        Regex regex;
        MarkovChain markovChain = null;

        public MainWindow()
        {
            InitializeComponent();

            markovChain = new MarkovChain();
            corpusFile = InitializeCorpusFile();            
        }

        private void DisplayTransitionMatrix()
        {
            lbViewNGramas.Items.Clear();

            foreach (var pair in markovChain.TransitionMatrix)
            {
                string word = pair.Key;
                List<Transition> transitions = pair.Value;

                string entry = $"\"{word}\":";
                foreach (var transition in transitions)
                {
                    entry += $" {transition.Key} ({transition.Concurrency} / {transition.Probability:F2}),";
                }

                lbViewNGramas.Items.Add(entry.TrimEnd(','));
            }
        }

        private string InitializeCorpusFile()
        {
            try
            {
                if (!File.Exists(pathCorpusFile))
                    return "";

                StringBuilder sb = new StringBuilder();
                using (StreamReader sr = new StreamReader(pathCorpusFile, Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.AppendLine(line);
                        markovChain.AddTransition(line);
                        markovChain.NormalizeProbabilities();
                        if (sb.Length > 10000)
                            break;
                    }
                    string c = sb.ToString();
                    DisplayTransitionMatrix();
                    return c;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al leer el archivo: " + ex.Message);
                return "";
            }
        }

        private string GetLastWord(string word)
        {
            if (!word.Contains(' '))
                return word;

            string isSpace = word.Substring(word.LastIndexOf(' ') + 1);
            regex = new Regex(@"[^\w\d]+");
            isSpace = regex.Replace(isSpace, string.Empty);

            //MessageBox.Show($"\"{isSpace}\"");
            return isSpace.ToLower();
        }

        private void txtInput_KeyUp(object sender, KeyEventArgs e)
        {
            string lastWord = GetLastWord(txtInput.Text);

            lbWordsPredict.Items.Clear();
                        
            var predicts = markovChain.GetNextWords(lastWord, int.Parse(txtLength.Text));
            foreach (string pre in predicts)
            {
                lbWordsPredict.Items.Add(pre);
            }
            
            //lbWordsPredict.Items.Add(markovChain.GenerateSentence(txtInput.Text, int.Parse(txtLength.Text), int.Parse(txtEpochs.Text)));
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(pathCorpusFile))
            {
                sw.Write(corpusFile);
                sw.WriteLine(txtInput.Text.ToLower());
            }

            // InitializeCorpusFile();
            // MessageBox.Show(corpusFile);

            markovChain.AddTransition(txtInput.Text);
            markovChain.NormalizeProbabilities();
            DisplayTransitionMatrix();
        }                

        #region NGramas por carácter
        private Dictionary<string, List<string>> BuildNGramasByChar(int numChar, string corpus)
        {
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();

                for (int i = 0; i < corpus.Length - numChar; i++)
                {
                    string secuencie = corpus.Substring(i, numChar);
                    if (!dic.ContainsKey(secuencie))
                        dic[secuencie] = new List<string>();
                    dic[secuencie].Add(corpus[i + numChar].ToString());
                }

                return dic;            
        }        

        //private string GetPredicts(string frase, int numChar, int epochs)
        //{
        //    string res = frase;
        //    Random rnd = new Random();

        //    for (int i = 0; i < epochs; i++)
        //    {
        //        if (!dicNGramas.ContainsKey(frase))
        //            break;
        //        List<string> car_posibles = dicNGramas[frase];
        //        string sig_car = car_posibles[rnd.Next(car_posibles.Count())];
        //        res += sig_car;
        //        frase = res.Substring(res.Length - numChar);
        //    }

        //    return res;
        //}
        #endregion

        #region NGramas por palabras        
        //private Dictionary<string, List<string>> BuildNGramasByWord(int words, string corpus)
        //{
        //    Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
        //    string[] tokens = Tokenization(corpus);

        //    for (int i = 0; i < tokens.Length-words; i++)
        //    {
        //        string secuence = "";
        //        for (int j = i; j < i+words; j++)
        //        {
        //            secuence += tokens[j] + " ";
        //        }   
        //        if (!dic.ContainsKey(secuence))
        //            dic[secuence] = new List<string>();
        //        dic[secuence].Add(tokens[i+words]);
        //    }

        //    return dic;
        //}

        //private string GetPredictsByWords(string frase, int num, int epochs)
        //{
        //    string res = frase;
        //    Random rnd = new Random();

        //    for (int i = 0; i < epochs; i++)
        //    {
        //        if (!dicNGramasWord.ContainsKey(frase))
        //            break;
        //        List<string> car_posibles = dicNGramasWord[frase];
        //        string sig_car = car_posibles[rnd.Next(car_posibles.Count())];
        //        res += " " + sig_car;
        //        string[] tokens = Tokenization(res);
        //        string secuence = "";
        //        for (int j = i; j < i + num; j++)
        //        {
        //            secuence += tokens[j] + " ";
        //        }
        //        frase = res.Substring(secuence.Length - num);
        //    }

        //    return res;
        //}
        #endregion
    }
}

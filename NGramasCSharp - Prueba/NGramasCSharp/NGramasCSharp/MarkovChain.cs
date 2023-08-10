using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NGramasCSharp
{
    public class MarkovChain
    {
        private Dictionary<string, List<Transition>> transitionMatrix;
        private char[] delimiters;
        private Random rnd;

        public Dictionary<string, List<Transition>> TransitionMatrix
        {
            get { return transitionMatrix; }
        }

        public MarkovChain()
        {
            transitionMatrix = new Dictionary<string, List<Transition>>();
            delimiters = new char[] { ' ', '.', ',', '!', '?', '\n', '\r' };
            rnd = new Random();
        }

        private string[] Tokenization(string sentence)
        {
            string[] tokens = sentence.ToLower().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            return tokens;
        }

        public void AddTransition(string sentence)
        {
            string[] tokens = Tokenization(sentence);

            for (int i = 0; i < tokens.Length - 1; i++)
            {
                string currentWord = tokens[i];
                string nextWord = tokens[i + 1];

                if (!transitionMatrix.ContainsKey(currentWord))
                    transitionMatrix[currentWord] = new List<Transition>();
                
                Transition currentTransition = transitionMatrix[currentWord].FirstOrDefault(t => t.Key == nextWord);
                if (currentTransition == null)
                    transitionMatrix[currentWord].Add(new Transition(1, nextWord));                                       
                else                
                    currentTransition.Concurrency++;                                                     
            }
        }

        public void NormalizeProbabilities()
        {
            foreach (var chain in transitionMatrix)
            {
                double totalTransitions = chain.Value.Sum(t => t.Concurrency);
                foreach (var transition in chain.Value)
                {
                    transition.Probability = transition.Concurrency / totalTransitions;
                }
            }
        }        

        public List<string> GetNextWords(string currentWord, int numWords)
        {
            List<string> result = new List<string>();

            if(!transitionMatrix.ContainsKey(currentWord))
                return result;

            var transitions = transitionMatrix[currentWord];
            var sortedTransitions = transitions.OrderByDescending(pair => pair.Probability);

            foreach (var pair in sortedTransitions)
            {
                string nextWord = pair.Key;
                if (nextWord != currentWord)
                    result.Add(nextWord);

                if (result.Count() >= numWords)
                    break;
            }

            return result;
        }

        //public string GenerateSentence(string startingWord, int maxLength, int epochs)
        //{
        //    if (!transitionMatrix.ContainsKey(startingWord))
        //        return startingWord;

        //    StringBuilder result = new StringBuilder();

        //    for (int epoch = 0; epoch < epochs; epoch++)
        //    {
        //        string currentWord = startingWord;
        //        List<string> sentence = new List<string> { currentWord };

        //        while (transitionMatrix.ContainsKey(currentWord) && sentence.Count < maxLength)
        //        {
        //            double diceRoll = rnd.NextDouble();
        //            double cumulativeProbability = 0;

        //            foreach (var pair in transitionMatrix[currentWord])
        //            {
        //                cumulativeProbability += pair.Value;
        //                if (diceRoll < cumulativeProbability)
        //                {
        //                    currentWord = pair.Key;
        //                    sentence.Add(currentWord);
        //                    break;
        //                }
        //            }
        //        }

        //        result.AppendLine(string.Join(" ", sentence));
        //    }

        //    return result.ToString();
        //}
    }
}

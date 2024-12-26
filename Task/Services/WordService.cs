using System;
using System.Collections.Generic;
using System.Linq;
using Task.Models;

namespace WordBoundingBoxAPI.Services
{
    public class WordService
    {
        public List<string> ProcessWords(List<Word> words)
        {
            
            var sortedWords = words
                .OrderBy(w => w.Bbox.Y)  
                .ThenBy(w => w.Bbox.X)  
                .ToList();

          
            var groupedByLine = sortedWords
                .GroupBy(w => w.Bbox.Y)
                .OrderBy(g => g.Key)  
                .ToList();

           
            var result = new List<string>();

            foreach (var group in groupedByLine)
            {
                var lineWords = group.Select(w => w.word).ToList();
                result.Add(string.Join(" ", lineWords));
            }

            return result;
        }
    }
}

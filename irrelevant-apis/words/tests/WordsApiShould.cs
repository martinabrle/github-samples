using System;
using Xunit;
using Words.Controllers;
using System.Linq;
using Words;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace tests
{
    public class WordsApiShould
    {
        private readonly WordsController  wordsController;

        public WordsApiShould()
        {
            // Arrange
            wordsController = new WordsController(null);
        }

        [Fact]
        public void ReturnAtLeastOneWord()
        {
            var actionResult = wordsController.GetAll();
            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult.Result);
            var resultObject = GetObjectResultContent<IEnumerable<Word>>(actionResult);
            Assert.NotNull(resultObject);
            Assert.NotEmpty(resultObject);
        }
        
        [Fact]
        public void CreateWord()
        {
            string tmpTestTerm = addRandomNewWord();

            var wordsActionResult = wordsController.GetAll();
            var words = GetObjectResultContent<IEnumerable<Word>>(wordsActionResult);

            int newWordsFoundAfterCreate = words.Where(e => e.Term.Equals(tmpTestTerm,StringComparison.OrdinalIgnoreCase)).Count();
            Assert.True(newWordsFoundAfterCreate == 1);

            wordsController.Delete(tmpTestTerm);
        }
        
        [Fact]
        public void GetWord()
        {
            string tmpTestTerm = addRandomNewWord();

            //Check the new word has been created
            var wordsActionResult = wordsController.GetAll();
            var words = GetObjectResultContent<IEnumerable<Word>>(wordsActionResult);
            int newWordsFoundAfterCreate = words.Where(e => e.Term.Equals(tmpTestTerm)).Count();
            Assert.True(newWordsFoundAfterCreate == 1);

            //Check Get found the new word
            var wordActionResult = wordsController.Get(tmpTestTerm);
            var word = GetObjectResultContent<Word>(wordActionResult);
            Assert.NotNull(word);
            Assert.Equal(word.Term, tmpTestTerm);

            //Delete the new word
            wordsController.Delete(tmpTestTerm);
        }
        
        [Fact]
        public void DeleteWord()
        {
            string tmpTestTerm = addRandomNewWord();

            //Check we have a new word in the data set
            var wordsActionResult = wordsController.GetAll();
            var words = GetObjectResultContent<IEnumerable<Word>>(wordsActionResult);
            int newWordsFoundAfterCreate = words.Where(e => e.Term.Equals(tmpTestTerm)).Count();
            Assert.True(newWordsFoundAfterCreate == 1);

            //Delete the new word
            wordsController.Delete(tmpTestTerm);

            //Check the new word has been deleted
            wordsActionResult = wordsController.GetAll();
            words = GetObjectResultContent<IEnumerable<Word>>(wordsActionResult);
            int newWordsFoundAfterDelete = words.Where(e => e.Term.Equals(tmpTestTerm)).Count();
            Assert.True(newWordsFoundAfterDelete == 0);
        }

        private string addRandomNewWord()
        {
            string tmpTestTerm = Guid.NewGuid().ToString();
            wordsController.Create(
                new Word()
                {
                    Term = tmpTestTerm,
                    Description = "delete_test_description",
                    CreatedDateTime = DateTime.UtcNow,
                    LastModifiedDateTime = DateTime.UtcNow
                });
            return tmpTestTerm;
        }

        private static T GetObjectResultContent<T>(ActionResult<T> result)
        {
            return (T)((ObjectResult)result.Result).Value;
        }
    }
}

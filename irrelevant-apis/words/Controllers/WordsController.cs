using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Words.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WordsController : ControllerBase
    {
        private static readonly List<Word> WeirdWords = new List<Word>()
        {
            new Word() {Term="absquatulate", Description="to leave somewhere abruptly", LastModifiedDateTime = DateTime.UtcNow, CreatedDateTime = DateTime.UtcNow  },
            new Word() {Term="canorous", Description="melodious or resonant", LastModifiedDateTime = DateTime.UtcNow, CreatedDateTime = DateTime.UtcNow  },
            new Word() {Term="hallux", Description="Anatomy: the big toe", LastModifiedDateTime = DateTime.UtcNow, CreatedDateTime = DateTime.UtcNow  },
            new Word() {Term="kylie", Description="Australien: a boomerang", LastModifiedDateTime = DateTime.UtcNow, CreatedDateTime = DateTime.UtcNow  },
            new Word() {Term="omophagy", Description="the eating of raw food, especially meat", LastModifiedDateTime = DateTime.UtcNow, CreatedDateTime = DateTime.UtcNow  },
            new Word() {Term="rawky", Description="foggy, damp, and cold", LastModifiedDateTime = DateTime.UtcNow, CreatedDateTime = DateTime.UtcNow  },
            new Word() {Term="serendipity", Description="happy and unexpected discovery", LastModifiedDateTime = DateTime.UtcNow, CreatedDateTime = DateTime.UtcNow  },
            new Word() {Term="gobbledygook", Description="text riddled with official jargon and overly complicated sentence structures", LastModifiedDateTime = DateTime.UtcNow, CreatedDateTime = DateTime.UtcNow  }
        };

        private readonly ILogger<WordsController> _logger;

        public WordsController(ILogger<WordsController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet] 
        public ActionResult<IEnumerable<Word>> GetAll()
        {
            return Ok(WeirdWords);
        }

        [HttpGet("{term}")] 
        public ActionResult<Word> Get(string term)
        {
            return Ok(WeirdWords.First(e => e.Term.Equals(term,StringComparison.OrdinalIgnoreCase)));
        }

        [HttpPost]
        public ActionResult Create(Word word)
        {
            if (!WeirdWords.Exists(e=>e.Term.Equals(word.Term,StringComparison.OrdinalIgnoreCase)))
            {
                word.CreatedDateTime = DateTime.UtcNow;
                word.LastModifiedDateTime = DateTime.UtcNow;
                WeirdWords.Add(word);
            }
            else
            {
                var existingWord = WeirdWords.First(e=>e.Term.Equals(word.Term,StringComparison.OrdinalIgnoreCase));
                existingWord.Description = word.Description;
                existingWord.LastModifiedDateTime = DateTime.UtcNow;
            }
            return Ok();
        }

        [HttpPatch]
        public ActionResult Patch(Word word)
        {
            return Accepted();
        }

        [HttpDelete]
        public ActionResult Delete(string term)
        {
            var wordToDelete = WeirdWords.Where(e => e.Term.Equals(term, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (wordToDelete != null)
                WeirdWords.Remove(wordToDelete);

            return Ok();
        }

    }
}

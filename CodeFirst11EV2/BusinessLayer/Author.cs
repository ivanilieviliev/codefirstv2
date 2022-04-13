using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer
{
    public class Author
    {
        [Key]
        public int ID { get; private set; }

        [Required, MaxLength(30)]
        public string Name { get; set; }

        public int Age { get; set; }

        public IEnumerable<Book> Books { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        private Author()
        {

        }

        public Author(string name, int age)
        {
            Name = name;
            Age = age;
        }
    }
}
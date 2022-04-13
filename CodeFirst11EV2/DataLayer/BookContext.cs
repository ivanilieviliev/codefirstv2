using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer
{
    public class BookContext : IDB<Book, string>
    {
        private LibraryDbContext _context;

        public BookContext(LibraryDbContext context)
        {
            _context = context;
        }

        public void Create(Book item)
        {
            try
            {
                Author authorFromDB = _context.Authors.Find(item.AuthorID);

                if (authorFromDB != null)
                {
                    item.Author = authorFromDB;
                }

                List<Genre> genres = new List<Genre>();

                foreach (var genre in item.Genres)
                {
                    Genre genreFromDB = _context.Genres.Find(genre.ID);

                    if (genreFromDB != null)
                    {
                        genres.Add(genreFromDB);
                    }
                    else
                    {
                        genres.Add(genre);
                    }
                }

                item.Genres = genres;

                _context.Books.Add(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Book Read(string key, bool useNavigationProperties = false)
        {
            try
            {
                IQueryable<Book> query = _context.Books.AsNoTrackingWithIdentityResolution();

                if (useNavigationProperties)
                {
                    query = query.Include(b => b.Author).Include(b => b.Genres);
                }

                return query.SingleOrDefault(b => b.ISBN == key);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Book> ReadAll(bool useNavigationProperties = false)
        {
            try
            {
                IQueryable<Book> query = _context.Books.AsNoTracking();

                if (useNavigationProperties)
                {
                    query = query.Include(b => b.Author).Include(b => b.Genres);
                }

                return query.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Book> Read(int skip, int take, bool useNavigationProperties)
        {
            try
            {
                IQueryable<Book> query = _context.Books.AsNoTrackingWithIdentityResolution();

                if (useNavigationProperties)
                {
                    query = query.Include(b => b.Author).Include(b => b.Genres);
                }

                return query.Skip(skip).Take(take).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(Book item, bool useNavigationProperties = false)
        {
            try
            {
                Book bookFromDB = Read(item.ISBN, useNavigationProperties);

                if (useNavigationProperties)
                {
                    bookFromDB.Author = item.Author;

                    List<Genre> genres = new List<Genre>();

                    foreach (Genre genre in item.Genres)
                    {
                        Genre genreFromDB = _context.Genres.Find(genre.ID);

                        if (genreFromDB != null)
                        {
                            genres.Add(genreFromDB);
                        }
                        else
                        {
                            genres.Add(genre);
                        }
                    }

                    bookFromDB.Genres = genres;
                }

                _context.Entry(bookFromDB).CurrentValues.SetValues(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(string key)
        {
            try
            {
                _context.Books.Remove(Read(key));
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

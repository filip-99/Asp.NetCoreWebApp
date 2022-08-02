﻿using my_books.Data.Models;
using my_books.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace my_books.Data.Services
{
    // Servis klasa je posrednik i ona komunicira sa kontekst klasom, koja komunicira sa bazom, a njene metode će okidati kontroler klasa BookControler
    public class BooksService
    {
        // Prvo će mo imati metodu za ubacivanje knjiga u bazi
        // Ovo će mo uraditi znači preko kontekst klase, jer ona komunicira sa bazom. Pa kreiramo najpre objekat, pa konstruktor sa ovim poljem
        private AppDbContext _context;  
        public BooksService(AppDbContext context)
        {
            _context=context;
        }

        // Pošto želimo da ubacimo podatke u bazu kreiramo metodu:
        public void AddBook(BookVM book)
        {
            // Kreiraćemo objekat modela knjige tj. Model Book
            // Možemo pisati Book ili var kao što je navedeno
            var _book = new Book()
            {
                Title = book.Title, 
                Description = book.Description,
                IsRead = book.IsRead,
                // Ovde će mo prvo proveriti da li je knjiga uopšte pročitana. Ako jeste unesi datum, ako nije biće null
                DateRead = book.IsRead ? book.DateRead.Value : null, 
                Genre = book.Genre,
                Author = book.Author,
                // Takođe i ovde je potrebno ispitati da li je knjiga najpre pročitana
                Rate = book.IsRead ? book.Rate.Value : null,
                Cover = book.Cover,
                DateAdded = DateTime.Now
            };
            // Sada kada smo uzeli prosleđene podatke funkciji, dodelili im vrednosti, potrebno je da ih prosledimo kontekst klasi
            _context.Books.Add(_book);
            _context.SaveChanges();
        }

        // Sada je potrebno da kreiramo metodu koja će prikazati sve podatke iz baze
        // Metoda je tipa List<> je vraća listu knjiga iz baze
        public List<Book> GetAllBooks()
        {
            // Imamo promenjivu var koja je tipa list, jer smo stavili da je jednak našoj bazi sa knjigama(_context.Books) konvertovanu u listu .ToList()
            var allBooks = _context.Books.ToList();
            return allBooks;
        }

        // Sada kreiramo metodu za odabir jedne knjige iz baze na osnovu prosleđenog ID-a
        public Book GetBookById(int bookId)
        {
            // Proveravamo bazu da li postoji knjiga koja je jednaka id-u prosleđenom u metodi
            // Funkcija FirstOrDefault ako ne uspe da nađe zadati ID vratiće kao razultat null vrednost (takođe je mogla da se koristi funkcija "First", ali ona će izbaciti grešku ako ne nađe ID)
            var book = _context.Books.FirstOrDefault(n => n.Id == bookId);
            return book;
        }

        // Sada kreiramo metodu za ažuriranje podataka u bazi
        // Ovoj metodi će biti prosleđen Id i novi podaci, koji su ograničeni u vju modelu i koje unosi korisnik
        public Book UpdateBookById(int bookId, BookVM book)
        {
            // Dakle proveravamo da li se ID prosleđen metodi nalazi u bazu podataka
            // Ako se nalazi sada će promenjiva _book referencirati na taj red u tabeli sa prosleđenim ID-em, u suprotnom imaće vrednost null
            var _book = _context.Books.FirstOrDefault(n => n.Id == bookId);
            // Ako ID postoji i nije null izvršiće se ažuriranje za taj red u tabeli čiji id je prosleđen metodi
            if(_book != null)
            {
                _book.Title = book.Title;
                _book.Description = book.Description;
                _book.IsRead = book.IsRead;
                _book.DateRead = book.IsRead ? book.DateRead.Value : null;
                _book.Genre = book.Genre;
                _book.Author = book.Author;
                _book.Rate = book.IsRead ? book.Rate.Value : null;
                _book.Cover = book.Cover;

                // Sačuvaćemo promene
                _context.SaveChanges();
            }
            // Na kraju metoda vraća apdejtovan red u tabeli
            return _book;
        }
    }
}

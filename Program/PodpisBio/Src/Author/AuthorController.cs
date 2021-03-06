﻿using PodpisBio.Src.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodpisBio.Src.Author
{
    class AuthorController
    {
        private List<Author> authors = new List<Author>();
        public SignatureController signatureController { get; }
        private AuthorService service;

        public AuthorController(SignatureController signatureController)
        {
            service = new AuthorService();
            this.signatureController = signatureController;
            initAuthorsFromDatabase();
        }

        //Przy starcie aplikacji, pobiera autorów wraz z danymi z bazy danych
        private void initAuthorsFromDatabase()
        {
            var authors = service.getAuthors();
            if (authors != null)
            {
                if (authors.Count.Equals(0)) { Debug.WriteLine("UWAGA Pobrana lista autorów z bazy jest pusta, czy powinna taka być?"); }
                else
                {
                    //Jeśli pobrano autorów, dodaj do listy
                    this.authors.AddRange(authors);
                    foreach(var author in authors)
                    {
                        foreach(var sign in author.getAllSignatures())
                        {
                            foreach(var stroke in sign.getStrokesOriginal())
                            {
                                //stroke.init();
                            }
                            //sign.init();
                            signatureController.addSignature(sign);
                        }
                        //Póki nie ma w bazie danych
                        author.calcWeights();
                    }
                }  
            }
            else { throw new Exception("Błąd pobierania autorów z bazy"); }
            
        }

        //Dodaje pustego autora (do bazy oraz lokalnie)
        public void addAuthor(String name)
        {
            Author author = new Author(name);
            author = service.postAuthor(author);
            if (author != null) { authors.Add(author); }          
        }

        //Zwraca listę imion autorów
        public List<String> getAuthorsNames()
        {
            List<String> names = new List<String>();

            foreach(var author in authors) { names.Add(author.getName()); }

            return names;
        }

        //Zwraca autora o zadanym imieniu
        public Author getAuthor(String name)
        {
            foreach(var author in authors)
            {
                if (author.getName().Equals(name))
                {
                    return author;
                }
            }
            return new Author();
        }

        //Sprawdza czy istnieje taki autor
        public bool isContaining(String name)
        {
            foreach(var author in authors)
            {
                if (author.getName().Equals(name)) { return true; }
            }
            return false;
        }

        public bool Empty()
        {
            if (authors.Count == 0)
                return true;
            bool allAuthorsEmpty = true;
            foreach (var autor in authors)
                if (!autor.EmptySignatures())
                    allAuthorsEmpty = false;
            return allAuthorsEmpty;
        }
    }
}

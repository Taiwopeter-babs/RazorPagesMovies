using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovies.Data;
using RazorPagesMovies.Models;

namespace RazorPagesMovies.Pages_Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovies.Data.RazorPagesMoviesContext _context;

        public IndexModel(RazorPagesMovies.Data.RazorPagesMoviesContext context)
        {
            _context = context;
        }

        public IList<Movie> Movies { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString {get; set;}

        public SelectList? Genres {get; set;}

        [BindProperty(SupportsGet = true)]
        public string? MovieGenre {get; set;}

        public async Task OnGetAsync()
        {
            // linq query to get all genres
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre select m.Genre;

            // linq query
            var movies = from m in _context.Movie select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(
                    m =>  m.Title != null && m.Title.ToLower().Contains(SearchString.ToLower())
                );
            }

            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(m => m.Genre == MovieGenre);
            }

            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            Movies = await movies.ToListAsync();
        }
    }
}

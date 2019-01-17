using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Books.Api.Filters;
using Books.Api.Models;
using Books.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IMapper _mapper;

        public BooksController(IBooksRepository booksRepository, IMapper mapper)
        {
            _booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [BooksResultFilter]
        public async Task<IActionResult> GetBooks()
        {
            var bookEntities = await _booksRepository.GetBooksAsync();
            return Ok(bookEntities);
        }

        [HttpGet]
        //[BookResultFilter]
        [BookWithCoversResultFilter]
        [Route("{id}", Name = "RouteGetBook")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var bookEntity = await _booksRepository.GetBookAsync(id);

            if (bookEntity == null)
            {
                return NotFound();
            }

            // book cover
            //var bookCover = await _booksRepository.GetBookCoverAsync("dummycover");
            var bookCovers = await _booksRepository.GetBookCoversAsync(id);

            //var propertyBag = new Tuple<Entities.Book, IEnumerable<ExternalModels.BookCover>>(bookEntity, bookCovers);
            //propertyBag.Item1;
            //propertyBag.Item2;

            //// using ValueTuple
            //(Entities.Book book, IEnumerable<ExternalModels.BookCover> bookCovers) propertyBag =
            //    (bookEntity, bookCovers);
            //propertyBag.bookCovers
            
            //return Ok((book: bookEntity, bookCovers: bookCovers));

            return Ok((bookEntity, bookCovers));

        }


        [HttpPost]
        [BookResultFilter]
        public async Task<IActionResult> CreateBook([FromBody] BookForCreation book)
        {
            var bookEntity = _mapper.Map<Entities.Book>(book);
            _booksRepository.AddBook(bookEntity);

            await _booksRepository.SaveChangesAsync();

            // fetch (refetch) the book from the data store, including the author
            await _booksRepository.GetBookAsync(bookEntity.Id);

            return CreatedAtRoute("RouteGetBook", new { id = bookEntity.Id }, bookEntity);
        }



    }
}

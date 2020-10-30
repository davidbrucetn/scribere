using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoogleApiService.Libs.Services
{
    public class GoogleBookService
    {
        private readonly IGetBook _getBook;

        public GoogleBookService(IGetBook getBook)
        {
            _getBook = getBook;
        }
        


        public async Task GetBookBasedOnSearch(string searchCriteria)
        {
            return await _getBook.ReturnBookBasedOnSearch(searchCriteria);
        }
    }
}

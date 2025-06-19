// MiMangaBot/Domain/Models/PaginationModels.cs
using System;

namespace MiMangaBot.Domain.Models
{
    public class PaginationParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }

    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public PaginationMetadata Pagination { get; set; }

        public PagedResult()
        {
            Data = new List<T>();
            Pagination = new PaginationMetadata();
        }

        public PagedResult(IEnumerable<T> data, int totalCount, int currentPage, int pageSize)
        {
            Data = data;
            Pagination = new PaginationMetadata
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = currentPage,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                CurrentPageCount = data.Count()
            };
        }
    }

    public class PaginationMetadata
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPageCount { get; set; }
        public bool HasNext => CurrentPage < TotalPages;
        public bool HasPrevious => CurrentPage > 1;
        public int? NextPage => HasNext ? CurrentPage + 1 : null;
        public int? PreviousPage => HasPrevious ? CurrentPage - 1 : null;
    }
}
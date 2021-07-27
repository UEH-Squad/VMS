using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.GenericRepository;

namespace VMS.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository _repository;

        public PostService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task AddPostWithCat(CreatePostWithCatViewModel postWithCatViewModel)
        {
            Post post = new()
            {
                Title = postWithCatViewModel.Title,
                Content = postWithCatViewModel.Content
            };

            Specification<Category> catSpec = new();
            catSpec.Conditions.Add(x => x.Name == postWithCatViewModel.CategoryName);
            Category cat = await _repository.GetAsync(catSpec);

            if (cat != null)
            {
                post.Categories = new List<Category>
                {
                    cat
                };

                await _repository.InsertAsync(post);
            }
        }

        public async Task DeletePost(int id)
        {
            Specification<Post> postSpec = new();
            postSpec.Conditions.Add(x => x.Id == id);
            Post post = await _repository.GetAsync(postSpec);
            await _repository.DeleteAsync(post);
        }

        public async Task<CreatePostWithCatViewModel> GetPostWithCat(int id)
        {
            if (id <= 0)
            {
                return new CreatePostWithCatViewModel();
            }

            Specification<Post> postSpec = new();
            postSpec.Conditions.Add(x => x.Id == id);
            Post post = await _repository.GetAsync(postSpec);

            List<Category> categories = await _repository.GetListAsync<Category>();

            CreatePostWithCatViewModel postWithCat = new()
            {
                Title = post.Title,
                Content = post.Content,
                Categories = categories
            };

            return postWithCat;
        }

        public async Task<List<PostViewModel>> PostsWithCategory()
        {
            Specification<Post> postsWithCatSpec = new()
            {
                Includes = post => post.Include(x => x.Categories)
            };

            List<Post> posts = await _repository.GetListAsync(postsWithCatSpec);

            IEnumerable<PostViewModel> postsWithCat = posts.Select(x => new PostViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Category = x.Categories.FirstOrDefault()?.Name
            });

            return postsWithCat.ToList();
        }

        public async Task UpdatePostWithCat(int id, CreatePostWithCatViewModel postWithCatViewModel)
        {
            Specification<Post> postSpec = new();
            postSpec.Conditions.Add(x => x.Id == id);
            Post post = await _repository.GetAsync(postSpec);

            Specification<Category> catSpec = new();
            catSpec.Conditions.Add(x => x.Name == postWithCatViewModel.CategoryName);
            Category cat = await _repository.GetAsync(catSpec);

            post.Title = postWithCatViewModel.Title;
            post.Content = postWithCatViewModel.Content;
            post.Categories = new List<Category>() { cat };

            await _repository.UpdateAsync(post);
        }
    }
}
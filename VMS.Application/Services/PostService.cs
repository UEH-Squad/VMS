using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.GenericRepository;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class PostService : BaseService, IPostService
    {
        public PostService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory) : base(repository, dbContextFactory)
        {
        }

        public async Task AddPostWithCat(CreatePostWithCatViewModel postWithCatViewModel)
        {
            Post post = new()
            {
                Title = postWithCatViewModel.Title,
                Content = postWithCatViewModel.Content
            };

            DbContext context = _dbContextFactory.CreateDbContext();

            Specification<Category> catSpec = new()
            {
                Conditions = new List<Expression<Func<Category, bool>>>
                {
                    x => x.Name == postWithCatViewModel.CategoryName
                }
            };
            Category cat = await _repository.GetAsync(context, catSpec);

            if (cat != null)
            {
                post.Categories = new List<Category>
                {
                    cat
                };

                await _repository.InsertAsync(context, post);
            }
        }

        public async Task DeletePost(int id)
        {
            DbContext context = _dbContextFactory.CreateDbContext();
            Specification<Post> postSpec = new()
            {
                Conditions = new List<Expression<Func<Post, bool>>>
                {
                    x => x.Id == id
                }
            };
            Post post = await _repository.GetAsync(context, postSpec);

            await _repository.DeleteAsync(context, post);
        }

        public async Task<CreatePostWithCatViewModel> GetPostWithCat(int id)
        {
            if (id <= 0)
            {
                return new CreatePostWithCatViewModel();
            }

            DbContext context = _dbContextFactory.CreateDbContext();

            Specification<Post> postSpec = new()
            {
                Conditions = new List<Expression<Func<Post, bool>>>
                {
                    x => x.Id == id
                }
            };
            Post post = await _repository.GetAsync(context, postSpec);

            List<Category> categories = await _repository.GetListAsync<Category>(context);

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
            DbContext context = _dbContextFactory.CreateDbContext();
            Specification<Post> postsWithCatSpec = new()
            {
                Includes = post => post.Include(x => x.Categories)
            };
            List<Post> posts = await _repository.GetListAsync(context, postsWithCatSpec);

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
            DbContext context = _dbContextFactory.CreateDbContext();

            Specification<Post> postSpec = new()
            {
                Includes = post => post.Include(x => x.Categories),
                Conditions = new List<Expression<Func<Post, bool>>>
                {
                    x => x.Id == id
                }
            };
            Post post = await _repository.GetAsync(context, postSpec);

            Specification<Category> catSpec = new()
            {
                Conditions = new List<Expression<Func<Category, bool>>>
                {
                    x => x.Name == postWithCatViewModel.CategoryName
                }
            };
            Category cat = await _repository.GetAsync(context, catSpec);

            post.Title = postWithCatViewModel.Title;
            post.Content = postWithCatViewModel.Content;
            post.Categories = new List<Category>() { cat };

            await _repository.UpdateAsync(context, post);
        }
    }
}
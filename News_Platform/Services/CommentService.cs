using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using News_Platform.DTOs;
using News_Platform.Models;
using News_Platform.Repositories;

namespace News_Platform.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IUserRepository _userRepository;

        public CommentService(ICommentRepository commentRepository, IArticleRepository articleRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _articleRepository = articleRepository;
            _userRepository = userRepository;
        }

        public async Task<List<CommentDto>> GetCommentsByArticleIdAsync(long articleId, long? userId)
        {
            var comments = await _commentRepository.GetCommentsByArticleIdAsync(articleId);

            return comments.Select(c => new CommentDto
            {
                CommentID = c.CommentID,
                ArticleID = c.ArticleID,
                UserID = c.UserID,
                ParentCommentID = c.ParentCommentID,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                AuthorName = c.User != null ? $"{c.User.FirstName} {c.User.LastName}" : "Unknown User",
                LikeCount = c.Likes.Count,
                IsLiked = userId.HasValue && c.Likes.Any(l => l.UserID == userId.Value)
            }).ToList();
        }






        public async Task<bool> AddCommentAsync(long articleId, long userId, string content, long? parentCommentId)
        {
            var article = await _articleRepository.GetArticleByIdAsync(articleId);
            if (article == null)
            {
                throw new KeyNotFoundException("Article not found.");
            }

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (parentCommentId.HasValue)
            {
                var parentComment = await _commentRepository.GetCommentByIdAsync(parentCommentId.Value);
                if (parentComment == null)
                {
                    throw new KeyNotFoundException("Parent comment not found.");
                }

                if (parentComment.ParentCommentID.HasValue)
                {
                    throw new InvalidOperationException("Nested replies are not allowed. You can only reply to top-level comments.");
                }
            }

            var comment = new Comment
            {
                ArticleID = articleId,
                UserID = userId,
                Content = content,
                ParentCommentID = parentCommentId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _commentRepository.AddCommentAsync(comment);
            return true;
        }


        public async Task<bool> DeleteCommentAsync(long commentId, long userId)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                return false;
            }

            if (comment.UserID != userId)
            {
                throw new UnauthorizedAccessException("You can only delete your own comments.");
            }

            await _commentRepository.DeleteCommentAsync(commentId);
            return true;
        }
    }
}

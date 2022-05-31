using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Pillow.ApplicationCore.Enums;
using Pillow.ApplicationCore.Exceptions;
using Pillow.ApplicationCore.Interfaces;

namespace Pillow.ApplicationCore.Entities.BookAggregate
{
    public class Book : BaseCodeEntity, IAggregateRoot
    {
        public DateTime Created { get; private set; }

        public string Title { get; private set; }

        public string CoverUri { get; private set; }

        public AccessLevel AccessLevel { get; private set; }

        public string Author { get; private set; }

        public DateTime PublishedDate { get; private set; }

        public bool IsPublished { get; private set; }

        public uint Likes { get; private set; }

        public uint Preorders { get; private set; }

        private readonly List<Track> _tracks = new List<Track>();

        public virtual IReadOnlyCollection<Track> Tracks => _tracks.AsReadOnly();

        private readonly List<BookTag> _bookTag = new List<BookTag>();

        public virtual IReadOnlyCollection<BookTag> BookTag => _bookTag.AsReadOnly();

        public Book(string code,
            string title,
            string coverUri,
            AccessLevel accessLevel,
            string author = null)
        {
            Code = code;
            Title = title;
            CoverUri = coverUri;
            AccessLevel = accessLevel;
            Author = author;
            Created = DateTime.UtcNow;
        }

        public void UpdateTitle(string title)
        {
            Guard.Against.NullOrEmpty(title, nameof(title));

            Title = title;
        }

        public void AddTag(string tagCode)
        {
            if (_bookTag.Any(bt => bt.BookCode == Code && bt.TagCode == tagCode))
            {
                throw new DublicateEntityException("This tag already belongs this book", tagCode);
            }

            _bookTag.Add(new BookTag(Code, tagCode));
        }

        public void RemoveTag(string tagCode)
        {
            Guard.Against.NullOrEmpty(tagCode, nameof(tagCode));

            BookTag tag = _bookTag.FirstOrDefault(t => t.TagCode == tagCode);
            Guard.Against.Null(tag, nameof(tag));

            _bookTag.Remove(tag);
        }

        public void AddTrack(Track track)
        {
            Guard.Against.Null(track, nameof(track));

            if (Tracks.Contains(track))
            {
                throw new DublicateEntityException("This track already exists in Tracks coolection this book", track.Code);
            }

            _tracks.Add(track);
        }

        public void RemoveTrack(Track track, string trackPath, bool isHardRemove)
        {
            Guard.Against.Null(track, nameof(track));

            if (!Tracks.Contains(track))
            {
                throw new TrackNotFoundException(track.Code);
            }

            if (isHardRemove)
            {
                if (System.IO.File.Exists(trackPath))
                {
                    System.IO.File.Delete(trackPath);
                }

                _tracks.Remove(track);
            }
            else
            {
                track.IsRemoved = true;
            }
        }

        public void Publish()
        {
            Guard.Against.NullOrEmpty(Tracks, nameof(Tracks));

            IsPublished = true;
            PublishedDate = DateTime.UtcNow;
        }

        public void CancelPublication()
        {
            IsPublished = false;
        }

        public void Like()
        {
            Likes++;
        }

        public void Unlike()
        {
            if (Likes == 0)
            {
                return;
            }

            Likes--;
        }

        public void AddPreorder()
        {
            Preorders++;
        }

        public void RemovePreorder()
        {
            if (Preorders == 0)
            {
                return;
            }

            Preorders--;
        }
    }
}
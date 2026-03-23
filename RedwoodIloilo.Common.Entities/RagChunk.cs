using System;
using Pgvector;

namespace RedwoodIloilo.Common.Entities
{
    public class RagChunk
    {
        public Guid Id { get; set; }

        public Guid RagDocumentId { get; set; }

        public Guid AppId { get; set; }

        // Order of the chunk within the document
        public int ChunkIndex { get; set; }

        public string Text { get; set; } = string.Empty;

        // Embedding stored as pgvector.Vector for pgvector mapping
        public Vector? Embedding { get; set; }

        public string? MetadataJson { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public RagDocument? Document { get; set; }
    }
}

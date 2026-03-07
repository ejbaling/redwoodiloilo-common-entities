using System;
using System.Collections.Generic;

namespace RedwoodIloilo.Common.Entities
{
    public class RagDocument
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Source { get; set; }

        // URI to original file stored in object storage (S3, Blob, etc.)
        public string? StorageUri { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Optional JSON metadata (stored as jsonb)
        public string? MetadataJson { get; set; }

        // Document-level tags for classification and routing (e.g., listing-level policies)
        // Stored as a Postgres text[] when using EF Core + Npgsql.
        public string[] Tags { get; set; } = Array.Empty<string>();

        // Navigation
        public ICollection<RagChunk> Chunks { get; set; } = new List<RagChunk>();
    }
}

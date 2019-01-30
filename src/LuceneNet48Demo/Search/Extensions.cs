using System;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace LuceneNet48Demo
{
	internal static class Extensions
	{
		/// <summary>
		/// for each index document for defined Term
		/// </summary>
		public static void ForEachTermDocs(this IndexWriter writer, Term whereTerm, string[] fields, Action<Document> todo)
		{
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (whereTerm == null)
				throw new ArgumentNullException(nameof(whereTerm));
			if (fields == null)
				throw new ArgumentNullException(nameof(fields));
			if (todo == null)
				throw new ArgumentNullException(nameof(todo));

			using (var reader = writer.GetReader(true))
			{
				var docs = MultiFields.GetTermDocsEnum(reader, null, whereTerm.Field, whereTerm.Bytes, DocsFlags.NONE);
				if (docs != null)
				{
					var docId = 0;
					while ((docId = docs.NextDoc()) != DocIdSetIterator.NO_MORE_DOCS)
					{
						var visitor = new DocumentStoredFieldVisitor(fields);
						reader.Document(docId, visitor);

						todo(visitor.Document);
					}
				}
			}
		}
	}
}

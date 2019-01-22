using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Util;

namespace LuceneNet48Demo
{
	/// <summary>
	/// Multifield query parser with alias support
	/// </summary>
	public class AliasMultiFieldQueryParser : MultiFieldQueryParser
	{
		protected IDictionary<string, string> m_aliases;

		#region Constructor

		public AliasMultiFieldQueryParser(LuceneVersion matchVersion, string[] fields, Analyzer analyzer, IDictionary<string, string> aliases, IDictionary<string, float> boosts)
		  : base(matchVersion, fields, analyzer)
		{
			this.m_aliases = aliases;
		}
		public AliasMultiFieldQueryParser(LuceneVersion matchVersion, string[] fields, Analyzer analyzer, IDictionary<string, string> aliases)
		 : base(matchVersion, null, analyzer)
		{
			this.m_aliases = aliases;
		}

		#endregion

		#region Field overrides

		protected override Query GetFieldQuery(string field, string queryText, int slop)
		{
			return base.GetFieldQuery(GetAlias(field), queryText, slop);
		}
		protected override Query GetFieldQuery(string field, string queryText, bool quoted)
		{
			return base.GetFieldQuery(GetAlias(field), queryText, quoted);
		}
		protected override Query GetFuzzyQuery(string field, string termStr, float minSimilarity)
		{
			return base.GetFuzzyQuery(GetAlias(field), termStr, minSimilarity);
		}
		protected override Query GetPrefixQuery(string field, string termStr)
		{
			return base.GetPrefixQuery(GetAlias(field), termStr);
		}
		protected override Query GetWildcardQuery(string field, string termStr)
		{
			return base.GetWildcardQuery(GetAlias(field), termStr);
		}
		protected override Query GetRangeQuery(string field, string part1, string part2, bool startInclusive, bool endInclusive)
		{
			return base.GetRangeQuery(GetAlias(field), part1, part2, startInclusive, endInclusive);
		}
		protected override Query GetRegexpQuery(string field, string termStr)
		{
			return base.GetRegexpQuery(GetAlias(field), termStr);
		}

		#endregion

		/// <summary>
		/// get alias for field
		/// </summary>
		private string GetAlias(string field)
		{
			if (string.IsNullOrEmpty(field))
				throw new ArgumentException(nameof(field));

			if (m_aliases.ContainsKey(field))
			{
				return m_aliases[field];
			}
			else
			{
				return field;
			}
		}
	}
}

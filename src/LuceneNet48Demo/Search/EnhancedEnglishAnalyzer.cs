using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.En;
using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Analysis.NGram;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Util;
using Lucene.Net.Util;

namespace LuceneNet48Demo
{
	/// <summary>
	/// Enhanced English Lucene analyzer
	/// </summary>
	/// <remarks>
	/// http://programagic.ca/blog/rest-api-lucenenet-part-2-a-few-tricks-and-tips
	/// My friends are visiting Montréal's engineering institutions -> my friend visit montreal engin institut
	/// </remarks>
	public class EnhancedEnglishAnalyzer : StopwordAnalyzerBase
	{
		#region Constructor

		public EnhancedEnglishAnalyzer(LuceneVersion matchVersion) : this(matchVersion, EnglishAnalyzer.DefaultStopSet)
		{
		}
		public EnhancedEnglishAnalyzer(LuceneVersion matchVersion, CharArraySet stopwords) : base(matchVersion, stopwords)
		{
		}
		public EnhancedEnglishAnalyzer(LuceneVersion matchVersion, TextReader stopwords) : base(matchVersion, LoadStopwordSet(stopwords, matchVersion))
		{
		}

		#endregion

		protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
		{
			Tokenizer source = new StandardTokenizer(m_matchVersion, reader);
			TokenStream result = new StandardFilter(m_matchVersion, source);
			// for stripping 's from words
			result = new EnglishPossessiveFilter(m_matchVersion, result);
			// converts é to e (and © to (c), etc.
			result = new ASCIIFoldingFilter(result);
			result = new LowerCaseFilter(m_matchVersion, result);
			result = new StopFilter(m_matchVersion, result, m_stopwords);
			// for chopping off common word suffixes, like removing ming from stemming, etc.
			result = new PorterStemFilter(result);
			result = new EdgeNGramTokenFilter(m_matchVersion, result, 3, 8);
			return new TokenStreamComponents(source, result);
		}
	}
}

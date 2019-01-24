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
	public class EnhEnglishAnalyzer : StopwordAnalyzerBase
	{
		private readonly bool _userNGram;
		private readonly int _ngramMin;
		private readonly int _ngramMax;

		#region Constructor

		/// <summary>
		/// MtG analyzer
		/// </summary>
		/// <param name="useNGram">use n-gram (https://en.wikipedia.org/wiki/N-gram)</param>
		/// <param name="ngramMin">Minimum size in codepoints of a single n-gram</param>
		/// <param name="ngramMax">Maximum size in codepoints of a single n-gram</param>
		public EnhEnglishAnalyzer(LuceneVersion matchVersion, bool useNGram = true, int ngramMin = 2, int ngramMax = 10) : base(matchVersion)
		{
			_userNGram = useNGram;
			_ngramMin = ngramMin;
			_ngramMax = ngramMax;
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
			result = new StopFilter(m_matchVersion, result, EnglishAnalyzer.DefaultStopSet);
			// for chopping off common word suffixes, like removing ming from stemming, etc.
			result = new PorterStemFilter(result);

			// The ngram tokenizer first breaks text down into words whenever it encounters one of a list of specified characters,
			// then it emits N-grams of each word of the specified length.
			if (_userNGram)
			{
				result = new EdgeNGramTokenFilter(m_matchVersion, result, _ngramMin, _ngramMax);
			}

			return new TokenStreamComponents(source, result);
		}
	}
}

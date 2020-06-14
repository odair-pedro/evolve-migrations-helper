using Xunit;

namespace Evolve.Migrations.Helper.Tests
{
    public class ProgramTests
    {
        private const string HeaderMessage = "Evolve Migrations Helper\nVersion: [0-9]+\\.[0-9]+\\.[0-9]+\n";
        
        private const string HelpMessagePattern =
            "Evolve Migrations Helper\nVersion: [0-9]+\\.[0-9]+\\.[0-9]+\n\n" +
            "Usage: migrations \\[command\\] \\[options\\]\n\n" +
            "Commands:\n" +
            "    add-dataset          Add a new migration file \\(on path: \"\\./datasets\"\\)\n" +
            "    add-migration        Add a new migration file \\(on path: \"\\./migrations\"\\)\n\n" +
            "Options:\n    -s\\|--separator" +
            "       The file name seperator. Default is '__' \\(Double underscore\\)\\. Eg: 'v[0-9]{14}__MyMigration\\.sql'\n";

        [Fact]
        public void Main_WithVersionArgs_ShoudPrintVersionMessage()
        {
            var printedText = OutputHelper.ExecuteAndReadOutputText(() => Program.Main(new[] { "--version" }));
            Assert.Matches(HeaderMessage, printedText);
        }
        
        [Fact]
        public void ValidateArguments_WithTwoArgs_ShouldntPrintMessage()
        {
            var printedText = OutputHelper.ExecuteAndReadOutputText(() => Program.ValidateArguments(new[] { "arg1", "arg2" }));
            Assert.Matches(string.Empty, printedText);
        }

        [Fact]
        public void ValidateArguments_WithTwoArgs_ShouldReturnTrue()
        {
            var result = Program.ValidateArguments(new[] { "arg1", "arg2" });
            Assert.True(result);
        }
        
        [Fact]
        public void ValidateArguments_WithEmptyArgs_ShouldPrintHelpMessage()
        {
            var printedText = OutputHelper.ExecuteAndReadOutputText(() => Program.ValidateArguments(new string[0]));
            Assert.Matches(HelpMessagePattern, printedText);
        }

        [Fact]
        public void ValidateArguments_WithEmptyArgs_ShouldReturnFalse()
        {
            var result = Program.ValidateArguments(new string[0]);
            Assert.False(result);
        }
        
        [Fact]
        public void ValidateArguments_WithHelpArg_ShouldPrintHelpMessage()
        {
            var printedText = OutputHelper.ExecuteAndReadOutputText(() => Program.ValidateArguments(new[] { "--help" }));
            Assert.Matches(HelpMessagePattern, printedText);
        }

        [Fact]
        public void ValidateArguments_WithHelpArg_ShouldReturnFalse()
        {
            var result = Program.ValidateArguments(new[] { "--help" });
            Assert.False(result);
        }

        [Fact]
        public void ValidateArguments_WithInvalidArgument_ShouldPrintInvalidArgMessage()
        {
            const string invalidArgsMessage = 
                "Invalid command.\n" +
                "Run 'migrations --help' for usage.\n";
            
            var printedText = OutputHelper.ExecuteAndReadOutputText(() => Program.ValidateArguments(new[] { "invalid-arg" }));
            Assert.Equal(invalidArgsMessage, printedText);
        }

        [Fact]
        public void ValidateArguments_WithInvalidArgument_ShouldReturnFalse()
        {
            var result = Program.ValidateArguments(new[] { "invalid-arg" });
            Assert.False(result);
        }
    }
}
using Xunit;

namespace GitBrowse.Test
{
    public class ProgramTests
    {

        // From git-fetch(5), native protocols:
        // ssh://[user@]host.xz[:port]/path/to/repo.git/
        // git://host.xz[:port]/path/to/repo.git/
        // http[s]://host.xz[:port]/path/to/repo.git/
        // ftp[s]://host.xz[:port]/path/to/repo.git/
        // [user@]host.xz:path/to/repo.git/ - scp-like but is an alternative to ssh.
        // [user@]hostalias:path/to/repo.git/ - handles host aliases defined in ssh_config(5)

        [Theory]
        [InlineData("ssh://user@github.com/path/to/repo.git", "https://github.com/path/to/repo")]
        [InlineData("https://github.com/path/to/repo.git", "https://github.com/path/to/repo")]
        [InlineData("http://github.com/path/to/repo.git/", "https://github.com/path/to/repo")]
        [InlineData("git@github.com:path/to/repo.git", "https://github.com/path/to/repo")]
        public void Program_ConvertGitUrl_TransformsConfiguredGitRemoteUrlCorrectlyToHttp(string input, string expected)
        {
            // Act
            var url = Program.GitRemoteToUrl(input);

            // Assert
            Assert.Equal(expected, url);
        }
    }
}

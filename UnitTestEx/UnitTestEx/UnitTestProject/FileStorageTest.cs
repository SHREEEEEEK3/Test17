using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using UnitTestEx;
using Assert = NUnit.Framework.Assert;

namespace UnitTestProject
{
    /// <summary>
    /// Summary description for FileStorageTest
    /// </summary>
    [TestClass]
    public class FileStorageTest
    {
        public const string MAX_SIZE_EXCEPTION = "DIFFERENT MAX SIZE";
        public const string NULL_FILE_EXCEPTION = "NULL FILE";
        public const string NO_EXPECTED_EXCEPTION_EXCEPTION = "There is no expected exception";
        public const string FILE_NAME_ALREADY_EXISTS_EXCEPTION = "FILE ALREADY EXIST";

        public const string SPACE_STRING = " ";
        public const string FILE_PATH_STRING = "@D:\\JDK-intellij-downloader-info.txt";
        public const string CONTENT_STRING = "Some text";
        public const string REPEATED_STRING = "AA";
        public const string WRONG_SIZE_CONTENT_STRING = "TEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtext";
        public const string TIC_TOC_TOE_STRING = "tictoctoe.game";

        public const int NEW_SIZE = 5;

        public FileStorage storage = new FileStorage(NEW_SIZE);

        /* ПРОВАЙДЕРЫ */

        static object[] NewFilesData =
        {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING) },
            new object[] { new File(SPACE_STRING, WRONG_SIZE_CONTENT_STRING) },
            new object[] { new File(FILE_PATH_STRING, CONTENT_STRING) }
        };

        static object[] FilesForDeleteData =
        {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING), REPEATED_STRING },
            new object[] { null, TIC_TOC_TOE_STRING }
        };

        static object[] NewExceptionFileData = {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING) }
        };

        static object[] LongData = 
        {
            new object[] { new File(REPEATED_STRING, WRONG_SIZE_CONTENT_STRING) }
        };

        static object[] NullData = {
            new object[] { new File(SPACE_STRING, CONTENT_STRING) }
        };

        /* Тестирование записи файла */
        [Test, TestCaseSource(nameof(NewFilesData))]
        public void WriteTest(File file)
        {
            try
            {
                storage.Write(file);
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine(MAX_SIZE_EXCEPTION);
            }
            catch (FileNameAlreadyExistsException e)
            {
                Console.WriteLine(FILE_NAME_ALREADY_EXISTS_EXCEPTION);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(NULL_FILE_EXCEPTION);
            }
            storage.DeleteAllFiles();
        }

        /* Тестирование записи дублирующегося файла */
        [Test, TestCaseSource(nameof(NewExceptionFileData))]
        public void WriteExceptionTest(File file) {
            bool isException = false;
            try
            {
                storage.Write(file);
                Assert.False(storage.Write(file));
            }
            catch (FileNameAlreadyExistsException)
            {
                Console.WriteLine(FILE_NAME_ALREADY_EXISTS_EXCEPTION);
                isException = true;
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine(MAX_SIZE_EXCEPTION);
            }
            Assert.True(isException, NO_EXPECTED_EXCEPTION_EXCEPTION);
            storage.DeleteAllFiles();
        }
  
        /* Тестирование проверки существования файла */
        [Test, TestCaseSource(nameof(NewFilesData))]
        public void IsExistsTest(File file) {
            string name = file.GetFilename();
            try
            {
                Assert.False(storage.IsExists(name));
                storage.Write(file);
                Assert.True(storage.IsExists(name));
            }
            catch (FileNameAlreadyExistsException e)
            {
                Console.WriteLine(FILE_NAME_ALREADY_EXISTS_EXCEPTION);
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine(MAX_SIZE_EXCEPTION);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(NULL_FILE_EXCEPTION);
            }
            storage.DeleteAllFiles();
        }

        /* Тестирование удаления файла */
        [Test, TestCaseSource(nameof(FilesForDeleteData))]
        public void DeleteTest(File file, String fileName) {  
            try
            {
                storage.Write(file);
                Assert.True(storage.Delete(fileName));
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(NULL_FILE_EXCEPTION);
            }
            Assert.False(storage.Delete(fileName));
        }

        /* Тестирование получения файлов */
        [Test]
        public void GetFilesTest()
        {
            foreach (File el in storage.GetFiles())
            {
                Assert.NotNull(el);
            }
        }

        /* Тестирование получения файла */
        [Test, TestCaseSource(nameof(NewFilesData))]
        public void GetFileTest(File expectedFile) 
        { 
            try
            {
                storage.Write(expectedFile);
                File actualfile = storage.GetFile(expectedFile.GetFilename());
                bool difference = actualfile.GetFilename().Equals(expectedFile.GetFilename()) && actualfile.GetSize().Equals(expectedFile.GetSize());
                Assert.IsTrue(difference, string.Format("There is some differences in {0} or {1}", expectedFile.GetFilename(), expectedFile.GetSize()));
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine(MAX_SIZE_EXCEPTION);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(NULL_FILE_EXCEPTION);
            }
            storage.DeleteAllFiles();
        }

        [Test, TestCaseSource(nameof(LongData))]
        public void outofmemmoryexception(File file)
        {
            try
            {
                storage.Write(file);
            }
            catch (OutOfMemoryException e)
            {
                NUnit.Framework.StringAssert.Contains(e.Message, e.Message);
                return;
            }
            Assert.Fail(NO_EXPECTED_EXCEPTION_EXCEPTION);
        }

        [Test, TestCaseSource(nameof(NullData))]
        public void nullreferenceexception(File file)
        {
            bool isException = false;
            try
            {
                storage.Write(file);
            }
            catch (NullReferenceException e)
            {
                NUnit.Framework.StringAssert.Contains(e.Message, e.Message);
                isException = true;
            }
            Assert.True(isException, NO_EXPECTED_EXCEPTION_EXCEPTION);
        }
    }
}

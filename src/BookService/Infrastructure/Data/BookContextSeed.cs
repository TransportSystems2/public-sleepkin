using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pillow.ApplicationCore.Entities.BookAggregate;
using Pillow.ApplicationCore.Entities.TagAggregate;
using Pillow.ApplicationCore.Enums;

namespace Pillow.Infrastructure.Data
{
    public class BookContextSeed
    {
        public static async Task SeedAsync(BookContext dbContext,
            ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                if (!await dbContext.Tags.AnyAsync())
                {
                    await dbContext.Tags.AddRangeAsync(
                        GetPreconfiguredTags());
                    
                    await dbContext.SaveChangesAsync();
                }

                if (!await dbContext.Books.AnyAsync())
                {
                    await dbContext.Books.AddRangeAsync(
                        GetPreconfiguredBooks());

                    await dbContext.SaveChangesAsync();
                }

                if (!await dbContext.Tracks.AnyAsync())
                {
                    await dbContext.Tracks.AddRangeAsync(
                        GetPreconfiguredTracks());

                    await dbContext.SaveChangesAsync();
                }

                if (!await dbContext.BookTag.AnyAsync())
                {
                    await dbContext.BookTag.AddRangeAsync(
                        GetPreconfiguredBookTag());

                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 5)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<BookContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(dbContext, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

        static IEnumerable<Tag> GetPreconfiguredTags()
        {
            return new List<Tag>
            {
                new Tag("awake", "????????????????????"),
                new Tag("sleep", "??????????????"),
                new Tag("baby", "????????????"),
                new Tag("boy", "????????????????"),
                new Tag("girl", "??????????????"),
                new Tag("russian-folk", "?????????????? ????????????????"),
                new Tag("modern", "??????????????????????"),
                new Tag("bogatyr", "?????? ??????????????????"),
                new Tag("lullaby", "??????????????????????"),
                new Tag("song", "??????????????"),
                new Tag("adventures", "??????????????????????"),
                new Tag("laziness", "????????"),
                new Tag("clever-and-stupid", "???? ?? ????????????????"),
                new Tag("naughty", "??????????????")
            };
        }

        static IEnumerable<Book> GetPreconfiguredBooks()
        {
            return new List<Book>
            {
                new Book("baba-yaga",
                    "???????? ??????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Free,
                    "?????????????? ????????????????"),

                new Book("carevna-laygushka",
                    "?????????????? ??????????????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Free,
                    "?????????????? ????????????????"),

                new Book("finist-yasnyi-sokol",
                    "???????????? ?????????? ??????????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Subscription,
                    "?????????????? ??.??."),

                new Book("gusi-lebedi",
                    "???????? ????????????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Subscription,
                    "???????????? ??.??."),

                new Book("ivan-carevich-i-seryi-volk",
                    "???????? ?????????????? ?? ?????????? ????????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Subscription,
                    "?????????????? ????????????????"),

                new Book("ivan-krestyanskiy-syn-i-chudo-yudo",
                    "???????? ???????????????????????? ?????? ?? ????????-??????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Free,
                    "?????????????????? ??.??."),

                new Book("kasha-iz-topora",
                    "???????? ???? ????????????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Free,
                    "?????????????? ??.??."),

                new Book("kolobok",
                    "??????????????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Free,
                    "???????????? ??.??."),

                new Book("koshey-bessmertnyi",
                    "?????????? ??????????????????????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Free,
                    "?????????? ??.??."),

                new Book("kurochka-ryaba",
                    "?????????????? ????????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Free,
                    "?????????????? ????????????????"),

                new Book("letuchiy-korabl",
                    "?????????????? ??????????????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Free,
                    "?????????????? ????????????????"),

                new Book("masha-i-medved",
                    "???????? ?? ??????????????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Free),

                new Book("morozko",
                    "??????????????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Free),

                new Book("repka",
                    "??????????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Free),

                new Book("sivka-burka",
                    "?????????? ??????????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Free),

                new Book("teremok",
                    "??????????????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Free),

                new Book("tri-medvedya",
                    "?????? ??????????????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Free),

                new Book("volk-i-semero-kozlyat",
                    "???????? ?? ???????????? ????????????",
                    "http://catalogbaseurltobereplaced/images/default.png",
                    AccessLevel.Free)
            };
        }

        static IEnumerable<Track> GetPreconfiguredTracks()
        {
            return new List<Track>
            {
                // ???????? ??????
                new Track("3cbdc340-b08b-4f25-9210-9da4ff523938", "baba-yaga", "???????? ??????", "????????????", "mp3", 2475470,
                    TimeSpan.FromSeconds(619)), // 1

                // ?????????????? ??????????????
                new Track("9b55ada4-35b6-452c-8b1f-cf41f30484b6", "carevna-laygushka", "?????????????? ??????????????", "????????????",
                    "mp3", 21836654, TimeSpan.FromSeconds(1365)), // 2

                // ???????????? ?????????? ??????????
                new Track("d0f87892-22eb-4c10-83e6-dccb9ef098c3", "finist-yasnyi-sokol", "???????????? ?????????? ??????????",
                    "??????????????", "mp3", 9278496, TimeSpan.FromSeconds(1160), true), // 3
                new Track("6527267a-d6b7-4d04-b2fa-27e21923228a", "finist-yasnyi-sokol", "", "????????????", "mp3", 8057718,
                    TimeSpan.FromSeconds(1151)), // 4
                new Track("6fcb1400-7e89-4cf8-9f5a-5ea711f4a2d2", "finist-yasnyi-sokol", "", "????????????????????????????", "mp3",
                    29450202, TimeSpan.FromSeconds(2454)), // 5

                // ???????? ????????????
                new Track("debd6311-12ae-4210-8b72-58e408671499", "gusi-lebedi", "????????????", "", "mp3", 8779134,
                    TimeSpan.FromSeconds(209)), // 6
            };
        }

        static IEnumerable<BookTag> GetPreconfiguredBookTag()
        {
            return new List<BookTag>
            {
                new BookTag("baba-yaga", "awake"),
                new BookTag("baba-yaga", "sleep"),
                new BookTag("baba-yaga", "russian-folk"),
                new BookTag("baba-yaga", "girl"),

                new BookTag("carevna-laygushka", "awake"),
                new BookTag("carevna-laygushka", "sleep"),
                new BookTag("carevna-laygushka", "russian-folk"),
                new BookTag("carevna-laygushka", "girl"),

                new BookTag("finist-yasnyi-sokol", "awake"),
                new BookTag("finist-yasnyi-sokol", "sleep"),
                new BookTag("finist-yasnyi-sokol", "russian-folk"),
                new BookTag("finist-yasnyi-sokol", "boy"),
                new BookTag("finist-yasnyi-sokol", "clever-and-stupid"),

                new BookTag("gusi-lebedi", "awake"),
                new BookTag("gusi-lebedi", "sleep"),
                new BookTag("gusi-lebedi", "russian-folk"),
                new BookTag("gusi-lebedi", "baby"),
                new BookTag("gusi-lebedi", "girl"),

                new BookTag("ivan-carevich-i-seryi-volk", "awake"),
                new BookTag("ivan-carevich-i-seryi-volk", "sleep"),
                new BookTag("ivan-carevich-i-seryi-volk", "russian-folk"),
                new BookTag("ivan-carevich-i-seryi-volk", "boy"),
                new BookTag("ivan-carevich-i-seryi-volk", "bogatyr"),
                new BookTag("ivan-carevich-i-seryi-volk", "adventures"),

                new BookTag("ivan-krestyanskiy-syn-i-chudo-yudo", "awake"),
                new BookTag("ivan-krestyanskiy-syn-i-chudo-yudo", "russian-folk"),
                new BookTag("ivan-krestyanskiy-syn-i-chudo-yudo", "boy"),
                new BookTag("ivan-krestyanskiy-syn-i-chudo-yudo", "bogatyr"),
                new BookTag("ivan-krestyanskiy-syn-i-chudo-yudo", "adventures"),
                new BookTag("ivan-krestyanskiy-syn-i-chudo-yudo", "clever-and-stupid"),

                new BookTag("kasha-iz-topora", "awake"),
                new BookTag("kasha-iz-topora", "russian-folk"),
                new BookTag("kasha-iz-topora", "boy"),
                new BookTag("kasha-iz-topora", "clever-and-stupid"),
                new BookTag("kasha-iz-topora", "naughty"),

                new BookTag("kolobok", "awake"),
                new BookTag("kolobok", "russian-folk"),
                new BookTag("kolobok", "baby"),
                new BookTag("kolobok", "clever-and-stupid"),

                new BookTag("koshey-bessmertnyi", "sleep"),
                new BookTag("koshey-bessmertnyi", "russian-folk"),
                new BookTag("koshey-bessmertnyi", "boy"),
                new BookTag("koshey-bessmertnyi", "bogatyr"),

                new BookTag("kurochka-ryaba", "sleep"),
                new BookTag("kurochka-ryaba", "russian-folk"),
                new BookTag("kurochka-ryaba", "baby"),

                new BookTag("letuchiy-korabl", "sleep"),
                new BookTag("letuchiy-korabl", "russian-folk"),
                new BookTag("letuchiy-korabl", "boy"),
                new BookTag("letuchiy-korabl", "girl"),
                new BookTag("letuchiy-korabl", "adventures"),

                new BookTag("masha-i-medved", "sleep"),
                new BookTag("masha-i-medved", "russian-folk"),
                new BookTag("masha-i-medved", "girl"),

                new BookTag("morozko", "sleep"),
                new BookTag("morozko", "russian-folk"),
                new BookTag("morozko", "girl"),
                new BookTag("morozko", "laziness"),

                new BookTag("repka", "russian-folk"),
                new BookTag("repka", "baby"),

                new BookTag("sivka-burka", "russian-folk"),
                new BookTag("sivka-burka", "girl"),

                new BookTag("teremok", "russian-folk"),
                new BookTag("teremok", "baby"),

                new BookTag("tri-medvedya", "russian-folk"),
                new BookTag("tri-medvedya", "girl"),

                new BookTag("volk-i-semero-kozlyat", "russian-folk"),
                new BookTag("volk-i-semero-kozlyat", "lullaby")
            };
        }
    }
}
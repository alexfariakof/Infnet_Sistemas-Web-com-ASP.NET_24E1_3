using Domain.Streaming.Agreggates;
using Domain.Streaming.ValueObject;
using Repository;

namespace DataSeeders.Implementations;
public class DataSeederMusic : IDataSeeder
{
    private readonly RegisterContext _context;
    public DataSeederMusic(RegisterContext context)
    {
        _context = context;
    }
    public void SeedData()
    {
        try
        {

            var band = new Band
            {
                Name = "Os Paralamas do Sucesso",
                Description = "Os Paralamas do Sucesso, também conhecida somente por Paralamas, é uma banda de rock brasileira formada em 1982 no município fluminense de Seropédica.",
                Backdrop = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/paralamas.jpg"
            };
            band.Genres = _context.Genre.Where(g => g.Name == "Rock").ToList();

            var playlist = new Playlist
            {
                Name = "Os Paralamas do Sucesso",
                Genres = _context.Genre.Where(g => g.Name == "Rock").ToList(),
                Backdrop = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/paralamas.jpg",
                Flats = _context.Flat.ToList(),
            };

            var musicList = new List<Music>
            {
                new Music
                {
                    Name = "Vital e Sua Moto",
                    Genres = _context.Genre.Where(g => g.Name == "Rock").ToList(),
                    Duration = new Duration(30),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Cinema+Mudo/01+-+Vital+E+Sua+Moto.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Foi o Mordomo",
                    Duration = new Duration(15),
                    Genres = _context.Genre.Where(g => g.Name == "Rock").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Cinema+Mudo/02+-+Foi+O+Mordomo.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Cinema Mudo",
                    Duration = new Duration(45),
                    Genres = _context.Genre.Where(g => g.Name == "Rock").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Cinema+Mudo/03+-+Cinema+Mudo.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Patrulha Noturna",
                    Duration = new Duration(5),
                    Genres = _context.Genre.Where(g => g.Name == "Rock").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Cinema+Mudo/04+-+Patrulha+Noturna.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Shopstake (Instrumental)",
                    Duration = new Duration(55),
                    Genres = _context.Genre.Where(g => g.Name == "Rock").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Cinema+Mudo/05+-+Shopstake+(Instrumental).mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Vovó Ondina é Gente Fina",
                    Duration = new Duration(55),
                    Genres = _context.Genre.Where(g => g.Name == "Rock").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Cinema+Mudo/06+-+Vov%C3%B3+Ondina+%C3%A9+Gente+Fina.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "O Que Eu Não Disse",
                    Duration = new Duration(55),
                    Genres = _context.Genre.Where(g => g.Name == "Rock").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Cinema+Mudo/07+-+O+Que+Eu+N%C3%A3o+Disse.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Química",
                    Duration = new Duration(55),
                    Genres = _context.Genre.Where(g => g.Name == "Rock").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Cinema+Mudo/08+-+Qu%C3%ADmica.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Encruzilhada",
                    Duration = new Duration(55),
                    Genres = _context.Genre.Where(g => g.Name == "Rock").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Cinema+Mudo/09+-+Encruzilhada.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Volúpia",
                    Duration = new Duration(55),
                    Genres = _context.Genre.Where(g => g.Name == "Rock").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Cinema+Mudo/10+-+Vol%C3%BApia.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                }
            };

            var album = new Album
            {
                Name = "Cinema Mudo",
                Musics = musicList,
                Backdrop = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Cinema+Mudo/front.jpg",
                Flats = _context.Flat.ToList()
            };

            band.AddAlbum(album);
            musicList.ForEach(m => m.Album = album);

            _context.Music.AddRange(musicList);
            _context.Add(album);
            _context.Add(band);
            _context.SaveChanges();

            musicList = new List<Music>
            {
                new Music
                {
                    Name = "Sinais do Sim",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Sinais+do+Sim/01+-+Sinais+Do+Sim.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Itaquaquecetuba",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Sinais+do+Sim/02+-+Itaquaquecetuba.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = " Medo do Medo",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Sinais+do+Sim/03+-+Medo+Do+Medo.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Não Posso Mais",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Sinais+do+Sim/04+-+N%C3%A3o+Posso+Mais.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Teu Olhar",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Sinais+do+Sim/05+-+Teu+Olhar.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Contraste",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Sinais+do+Sim/06+-+Contraste.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Cuando Pase El Temblor",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Sinais+do+Sim/07+-+Cuando+Pase+El+Temblor.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Corredor",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Sinais+do+Sim/08+-+Corredor.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Blow The Wind",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Sinais+do+Sim/09+-+Blow+The+Wind.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Olha a Gente Aí",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Sinais+do+Sim/10+-+Olha+A+Gente+A%C3%AD.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Sempre Assim",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Sinais+do+Sim/11+-+Sempre+Assim.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                }
            };

            album = new Album
            {
                Name = "Sinais do Sim",
                Genres =  _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                Musics = musicList,
                Backdrop = "https://playlist-music.s3.sa-east-1.amazonaws.com/Os+Paralamas+do+Sucesso/Sinais+do+Sim/front.jpg",
                Flats = _context.Flat.ToList()
            };

            band.AddAlbum(album);
            musicList.ForEach(m => m.Album = album);
            _context.Music.AddRange(musicList);
            _context.SaveChanges();


            band = new Band
            {
                Name = "Legião Urbana",
                Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                Description = "Legião Urbana foi uma banda brasileira de rock formada em 1982, em Brasília, por Renato Russo e Marcelo Bonfá, que contou com Dado Villa-Lobos e Renato Rocha em sua formação mais conhecida.",
                Backdrop = "https://playlist-music.s3.sa-east-1.amazonaws.com/Legi%C3%A3o+Urbana/Legi%C3%A3o_Urbana.jpg"
            };

            playlist = new Playlist
            {
                Name = "Legião Urbana",
                Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                Backdrop = "https://playlist-music.s3.sa-east-1.amazonaws.com/Legi%C3%A3o+Urbana/Legi%C3%A3o_Urbana.jpg",
                Flats = _context.Flat.ToList(),
            };

            musicList = new List<Music>
            {
                new Music
                {
                    Name = "Tempo Perdido",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Legi%C3%A3o+Urbana/Perfil/01.+Tempo+Perdido.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Será",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Legi%C3%A3o+Urbana/Perfil/02.+Ser%C3%A1.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Quase Sem Querer",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Legi%C3%A3o+Urbana/Perfil/03.+Quase+Sem+Querer.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Indios",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Legi%C3%A3o+Urbana/Perfil/04.+Indios.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = " Ha Tempos",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Legi%C3%A3o+Urbana/Perfil/05.+Ha+Tempos.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Perfeicao",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Legi%C3%A3o+Urbana/Perfil/06.+Perfeicao.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Metal Contra as Nuvens",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Legi%C3%A3o+Urbana/Perfil/07.+Metal+Contra+as+Nuvens.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Pais e Filhos",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Legi%C3%A3o+Urbana/Perfil/08.+Pais+e+Filhos.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "O Teatro dos Vampiros",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Legi%C3%A3o+Urbana/Perfil/09.+O+Teatro+dos+Vampiros.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Giz",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Legi%C3%A3o+Urbana/Perfil/10.+Giz.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Vento no Litoral",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Legi%C3%A3o+Urbana/Perfil/11.+Vento+no+Litoral.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Que Pais E Este",
                    Duration = new Duration(30),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Legi%C3%A3o+Urbana/Perfil/13.+Que+Pais+E+Este.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
                new Music
                {
                    Name = "Eduardo e Monica",
                    Duration = new Duration(30),
                    Genres = _context.Genre.Where(g => g.Name == "Rock" || g.Name == "Popular").ToList(),
                    Url = "https://playlist-music.s3.sa-east-1.amazonaws.com/Legi%C3%A3o+Urbana/Perfil/14.+Eduardo+e+Monica.mp3",
                    Playlists = { playlist },
                    Flats = _context.Flat.Where(f => f.Id == new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa5")).ToList()
                },
            };

            album = new Album
            {
                Name = "Perfil Legião Urbana",
                Musics = musicList,
                Backdrop = "https://playlist-music.s3.sa-east-1.amazonaws.com/Legi%C3%A3o+Urbana/Perfil/Front.jpg",
                Flats = _context.Flat.ToList()
            };


            band.AddAlbum(album);
            musicList.ForEach(m => m.Album = album);
            _context.Music.AddRange(musicList);
            _context.Add(album);
            _context.Add(band);
            _context.SaveChanges();
        }
        catch
        {
            Console.WriteLine($"Dados já cadastrados na base de dados");
        }
    }
}
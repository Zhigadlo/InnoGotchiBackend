using InnnoGotchi.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnnoGotchi.DAL.EF.EntityTypeConfiguration
{
    public class PictureConfiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Name);
            builder.Property(p => p.Image);
            builder.Property(p => p.Description);

            builder.HasData(GetPictures().ToArray());
        }

        private byte[] GetBytesFromImage(string path)
        {
            using (FileStream fsstream = File.OpenRead(path))
            {
                byte[] buffer = new byte[fsstream.Length];
                fsstream.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        private List<Picture> GetPictures()
        {
            List<Picture> pictures = new List<Picture>();

            string rootPath = Directory.GetParent(Environment.CurrentDirectory)?.FullName + "\\InnnoGotchi.DAL\\EF\\EntityTypeConfiguration";
            int id = 1;
            for (int i = 1; i < 6; i++)
            {
                string picPath = rootPath + "\\BodyPartPictures\\Bodies\\body" + i + ".svg";
                pictures.Add(new Picture()
                {
                    Id = id,
                    Name = "body" + i,
                    Description = "Body",
                    Image = GetBytesFromImage(picPath)
                });
                id++;
            }

            for (int i = 1; i < 7; i++)
            {
                string picPath = rootPath + "\\BodyPartPictures\\Eyes\\eyes" + i + ".svg";
                pictures.Add(new Picture()
                {
                    Id = id,
                    Name = "eyes" + i,
                    Description = "Eyes",
                    Image = GetBytesFromImage(picPath)
                });
                id++;
            }

            for (int i = 1; i < 6; i++)
            {
                string picPath = rootPath + "\\BodyPartPictures\\Mouths\\mouth" + i + ".svg";
                pictures.Add(new Picture()
                {
                    Id = id,
                    Name = "mouth" + i,
                    Description = "Mouth",
                    Image = GetBytesFromImage(picPath)
                });
                id++;
            }

            for (int i = 1; i < 7; i++)
            {
                string picPath = rootPath + "\\BodyPartPictures\\Noses\\nose" + i + ".svg";
                pictures.Add(new Picture()
                {
                    Id = id,
                    Name = "nose" + i,
                    Description = "Nose",
                    Image = GetBytesFromImage(picPath)
                });
                id++;
            }

            return pictures;
        }
    }
}

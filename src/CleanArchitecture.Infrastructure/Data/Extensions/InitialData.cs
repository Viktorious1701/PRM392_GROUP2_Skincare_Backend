namespace CleanArchitecture.Infrastructure.Data.Extensions;

internal class InitialData
{
  public static IEnumerable<User> Users => new List<User>
  {
    new()
    {
      Id = new Guid("8E62D9EE-64AF-47E7-8A49-FA1E2F5DA054"),
      UserName = "Admin123",
      Email = "admin123@gmail.com",
      EmailConfirmed = true,
      BirthDate = new DateOnly(2000, 1, 1),
      FirstName = "John",
      LastName = "Doe",
      Gender = true,
      PhoneNumber = "0901234567"
    },
    new()
    {
      Id = new Guid("D650E6F2-C402-45C1-B92F-F7573BDE7335"),
      UserName = "MinhAnh2610",
      Email = "minhanh26102004@gmail.com",
      EmailConfirmed = true,
      BirthDate = new DateOnly(2000, 1, 1),
      FirstName = "Pham",
      LastName = "Anh",
      Gender = true,
      PhoneNumber = "0903234967"
    },
    new User
    {
      Id = new Guid("103E55BA-607A-4D27-8119-B0C22969F02A"),
      UserName = "AliceJ",
      FirstName = "Alice",
      LastName = "Johnson",
      BirthDate = new DateOnly(1985, 6, 15),
      Gender = true,
      Email = "alice.johnson@example.com",
      PhoneNumber = "0921234297"
    },
    new User
    {
      Id = new Guid("518F5719-3E70-4AE2-BD92-B7FDC7A84DA2"),
      UserName = "BobS",
      FirstName = "Bob",
      LastName = "Smith",
      BirthDate = new DateOnly(1990, 3, 22),
      Gender = true,
      Email = "bob.smith@example.com",
      PhoneNumber = "0901534067"
    },
    new User
    {
      Id = new Guid("70FF9BBF-BD8D-46BE-AE26-772C74973276"),
      UserName = "CharlieD",
      FirstName = "Charlie",
      LastName = "Davis",
      BirthDate = new DateOnly(1988, 9, 10),
      Gender = true,
      Email = "charlie.davis@example.com",
      PhoneNumber = "0981122334"
    },
    new User
    {
      Id = new Guid("A1E34814-7E85-4858-B0AB-2D9E2DA27D99"),
      UserName = "EmmaW",
      FirstName = "Emma",
      LastName = "Williams",
      BirthDate = new DateOnly(1995, 12, 5),
      Gender = false,
      Email = "emma.williams@example.com",
      PhoneNumber = "0798899001"
    },
    new User
    {
      Id = new Guid("2AD907D6-65D9-4A8F-B662-FA5EDF293FC6"),
      UserName = "DanielB",
      FirstName = "Daniel",
      LastName = "Brown",
      BirthDate = new DateOnly(2000, 7, 19),
      Gender = true,
      Email = "daniel.brown@example.com",
      PhoneNumber = "0934567890"
    }
  };

  public static IEnumerable<Role> Roles => new List<Role>
  {
    new Role { Id = new Guid("55535483-D1EC-459C-8189-C6D8C4B50195"), Name = "Customer" },
    new Role { Id = new Guid("C459402C-64CB-4D36-9238-60B15462CE25"), Name = "Staff" },
    new Role { Id = new Guid("D586839A-B80C-4EBB-8488-95CDD857D1BF"), Name = "Manager" }
  };
  // Add new static property for Store data
  public static IEnumerable<Store> Stores => new List<Store>
    {
        new Store
        {
            Id = new Guid("1f9b3070-5527-4a0b-9a67-a068a658a5c3"),
            Name = "De Fleur Flagship Store",
            Address = "123 Le Loi, District 1, Ho Chi Minh City",
            Latitude = 10.7752,
            Longitude = 106.7019,
            PhoneNumber = "028-1111-2222",
            OpeningHours = "9:00 AM - 10:00 PM"
        },
        new Store
        {
            Id = new Guid("8e217e3a-e998-44e5-905d-1616f7f25977"),
            Name = "De Fleur Crescent Mall",
            Address = "101 Ton Dat Tien, Tan Phu, District 7, Ho Chi Minh City",
            Latitude = 10.7290,
            Longitude = 106.7161,
            PhoneNumber = "028-3333-4444",
            OpeningHours = "10:00 AM - 9:30 PM"
        },
        new Store
        {
            Id = new Guid("a4f4b4e2-632b-4e1d-8c44-59e5e2e8e4c7"),
            Name = "De Fleur Vincom Center",
            Address = "72 Le Thanh Ton, Ben Nghe, District 1, Ho Chi Minh City",
            Latitude = 10.7783,
            Longitude = 106.7025,
            PhoneNumber = "028-5555-6666",
            OpeningHours = "9:30 AM - 10:00 PM"
        }
    };
  public static IEnumerable<Batch> Batches
  {
    get
    {
      var cosmetics = Cosmetics.ToList();
      var batches = new List<Batch>();

      var guids = new List<Guid>()
      {
        new Guid("400E3B1B-3B67-4470-A233-4D9517A08A2E"),
        new Guid("EECE7608-65AE-48EF-B5DE-794DF488C109"),
        new Guid("FD3DA231-FFB3-430D-931C-D018031AF56E"),
        new Guid("D4763EB0-302E-4D8F-A2E3-051B03386F6B"),
        new Guid("179783E5-1FFA-4D7B-8253-A8AD75FB6642"),
        new Guid("6F43B761-004B-47BC-878B-FE9CFD1C888F"),
        new Guid("835466E8-B0E7-4F7F-8973-A763508939A4"),
        new Guid("F38520AE-6044-44F2-A81A-209044366944"),
        new Guid("02FBEA2C-E703-473E-8826-C5E2212E89AA"),
        new Guid("898832E3-3040-4D36-9F33-334298ECF671"),
        new Guid("3AA9C419-E1C6-4465-9D1E-C49170EEE6FC"),
        new Guid("EFBD2B5A-7ACC-46C6-8C1B-E01F2F5B130B"),
        new Guid("7EF8E750-BDFE-4B23-92EA-1A6B4FDBB45A"),
        new Guid("0F8AC791-DAD4-4A37-BD41-36C315942856"),
        new Guid("C75D3063-7C41-418C-8DC1-0987ED8B914B"),
        new Guid("CEF6CD26-73F2-4815-A2C2-21C857378D9D"),
        new Guid("85B7F538-8F31-4DF2-B151-0FBCA9F68821"),
        new Guid("D1077337-3473-460F-9687-63FC99522B34"),
        new Guid("DA1DDB7F-CBD7-4990-915A-7289DB596D46"),
        new Guid("DD45E94C-5534-4FC9-8F54-B0AC71D56D80"),
        new Guid("597D09F9-66FD-4524-AEE0-DF8535E87DC3"),
        new Guid("28C01AA5-E9FB-4594-B6F3-92CC85760763"),
        new Guid("7ED5F2B8-17B1-44FD-BD50-B6E80E602ACC"),
        new Guid("4C9208AF-48EB-4A83-8590-6E1FCFADC779"),
        new Guid("969BAEB2-AB92-41CE-B0EB-78D039FEFCE0"),
        new Guid("31A16920-F9C3-4101-9A28-497B3BAF30F3"),
        new Guid("B5A35586-D0D6-47ED-BA07-42D806573CCB"),
        new Guid("83F4B531-5D40-40E5-9972-E93163BAD11E"),
        new Guid("4556C015-A183-4C8A-9902-D5E88A97D918"),
        new Guid("E2E3A5B9-55FB-4193-8583-E39004BE0680"),
        new Guid("25B1534A-6869-4F95-A889-E4A0B0BD614B"),
        new Guid("02FA7395-A7E9-4A04-97A5-9F6F41FCC23C"),
        new Guid("12F1588A-F501-45BC-B636-1EA54FE0823C"),
        new Guid("B90E705F-6837-4852-9242-37F81FE71801"),
        new Guid("8C3D5C9B-7440-4635-AB71-5E5525A0F88C"),
        new Guid("12A47FA7-F8F1-4C79-A918-B9A033C06E1C"),
        new Guid("D5B96A1F-44D7-45B6-93BB-EB5153F22B0C"),
        new Guid("341E897F-D26E-4F55-853F-B20B3950A04B"),
        new Guid("FBF3821D-E7CB-44D9-9AA6-0CE8389C8CE0"),
        new Guid("F989AA6F-8EA5-488D-A246-97A42F229124"),
        new Guid("C6084C56-E5E1-4787-A017-CB02CEB809BC"),
        new Guid("6B8F1493-867B-4595-833B-2E2F05C60AC0"),
        new Guid("B0E1AAE2-4917-4558-9BB9-43C2E266501C"),
        new Guid("9CA7271B-1E9F-4C53-89C9-E0E5E63A5D1A"),
        new Guid("58372E1D-3E57-4A22-9FB0-2AB74CA0487A"),
        new Guid("781A2F74-95B0-46F8-8D1E-1779D34F7FBA"),
        new Guid("83FB16CB-00F4-47BC-95E9-0240B5D0FAC7"),
        new Guid("40E39108-A5E6-4F7D-A4D0-EB5E08716551"),
        new Guid("D80EA7F5-A1B9-49D9-B706-F4AAA77C33D7"),
        new Guid("BAF00AA9-47F4-4A1C-B291-0CBF24EBF824"),
        new Guid("5B0D4B34-A33F-41BF-A2BB-A2C10C08533F"),
        new Guid("74CEC1FF-4173-4560-8410-2D90037ADC6B"),
        new Guid("57AA548A-013B-42E5-8DC9-C62739088E98"),
        new Guid("F7530931-AB4F-482B-8FED-9C72EF98823C"),
        new Guid("60F899B4-628A-46B4-A0C0-79D5CFF4DEDD"),
        new Guid("DDB2AFDA-CABC-446E-9DE5-31C241623B04"),
        new Guid("4803E7C9-E413-4B72-8CFF-CEC5D3C7A31F"),
        new Guid("5DE0313C-50F4-4449-9A1A-B47A1D8BE375"),
        new Guid("CA4FAF4F-4811-4F18-9500-02C45937FE09"),
        new Guid("F23F14DE-0532-4294-92A3-198F9F93E19A"),
        new Guid("20A31F54-8D83-4C58-89A7-C6BF0AAF00D7"),
        new Guid("FA8EEB49-EC19-4FAD-86E2-D9EE371D838C"),
        new Guid("28925811-DB8C-4866-9FF4-33584072904C"),
        new Guid("CCA29215-411A-4E3E-998A-57637EBABFD0"),
        new Guid("8E7D68AD-7953-40DD-BD6D-E681C1C64CF8"),
        new Guid("92FD68A6-7678-4F93-B2ED-B362D37301DC"),
        new Guid("DE9AC9D8-C3D6-44FB-A726-7273B7D3CB37"),
        new Guid("1A3904C0-4817-4E44-89B1-33988AE82B83"),
        new Guid("D9276BE3-CC19-484D-A230-33028F87B7BF"),
        new Guid("27F2E0DA-493C-4F48-9E96-A402C78ACE1E"),
        new Guid("5FB79059-33D8-46D9-9D60-3C791F0C5F87"),
        new Guid("343E8930-FD95-47DF-B583-49E7CC41FE2C"),
        new Guid("4C6E048E-E6B2-460A-9734-436F450F1743"),
        new Guid("3CAC9688-C6F4-49EA-AD53-0119F11DA0F3"),
        new Guid("F4EF5A0E-99F2-4C65-88AE-DA5EBAF6C85E"),
        new Guid("0500755B-13E8-47E4-A13A-9B0E64741282"),
        new Guid("E2151F87-5181-4AD6-9286-C6AAD0D11908"),
      };
      var guidIndex = 0;

      foreach (var cosmetic in cosmetics)
      {
        batches.Add(new Batch
        {
          Id = guids[guidIndex++],
          CosmeticId = cosmetic.Id,
          Quantity = 200,
          ManufactureDate = new DateOnly(2025, 3, 15),
          ExportedDate = new DateOnly(2025, 4, 1),
          ExpirationDate = new DateOnly(2027, 3, 15),
        });
      }

      return batches;
    }
  }

  public static IEnumerable<Blog> Blogs
  {
    get
    {
      var staffs = Users.ToList();
      return new List<Blog>
      {
        new Blog
        {
          Id = new Guid("CB64A508-526E-4156-A512-1D1BF7A5A032"),
          StaffId = staffs[3].Id,
          Title = "Skincare Education: Master the Basics",
          Content =
            "Discover essential skincare tips, tutorials, and best practices to keep your skin healthy and radiant. Our experts share insights on proper cleansing, moisturizing, and sun protection to help you build a solid skincare foundation.",
          BlogTags = new List<BlogTag>()
        },
        new Blog
        {
          Id = new Guid("C9DED966-A2F7-4328-B899-8201508D5476"),
          StaffId = staffs[3].Id,
          Title = "Routine Building & Ingredient Guide for Every Skin Concern",
          Content =
            "Building your perfect skincare routine is easier than ever. Learn how to select the right ingredients for your unique skin concerns and get expert advice on customizing a regimen to combat dryness, oiliness, and signs of aging.",
          BlogTags = new List<BlogTag>()
        },
        new Blog
        {
          Id = new Guid("BAADC4E3-EE7A-44BB-9942-33B3C63B8DA4"),
          StaffId = staffs[3].Id,
          Title = "Deep Dive into Active Ingredients: Vitamin C & Retinol",
          Content =
            "Explore the benefits and science behind active ingredients like Vitamin C and Retinol. This post details how these potent ingredients can transform your skincare routine and improve overall skin health.",
          BlogTags = new List<BlogTag>()
        },
        new Blog
        {
          Id = new Guid("7E15DA89-51F2-4F64-9603-84065B2757C0"),
          StaffId = staffs[3].Id,
          Title = "How to Build a Customized Skincare Routine",
          Content =
            "Every skin is unique. Learn a step-by-step approach to creating a personalized skincare routine that meets your skin’s specific needs, including product recommendations and application techniques.",
          BlogTags = new List<BlogTag>()
        },
        new Blog
        {
          Id = new Guid("384D9D37-F554-4EA1-8443-6C4054F3DFB8"),
          StaffId = staffs[3].Id,
          Title = "Managing Acne and Other Skin Concerns",
          Content =
            "From acne to hyperpigmentation, this post offers actionable strategies and product tips to address common skin concerns. Learn how to choose the right treatments to restore balance and clarity.",
          BlogTags = new List<BlogTag>()
        },
        new Blog
        {
          Id = new Guid("F0632A67-8571-4E6A-A6F0-E22704D1FC42"),
          StaffId = staffs[3].Id,
          Title = "Expert Advice: Dermatologist Tips for Healthy Skin",
          Content =
            "Get professional advice straight from dermatologists. This post covers the latest treatments, daily routines, and lifestyle tips to maintain healthy, glowing skin.",
          BlogTags = new List<BlogTag>()
        },
        new Blog
        {
          Id = new Guid("55555555-5555-5555-5555-555555555555"),
          StaffId = staffs[3].Id,
          Title = "The Ultimate Guide to Sunscreen Application",
          Content =
            "Sunscreen is your best defense against UV damage. Learn how to choose the right SPF, application techniques, and tips for keeping your skin protected throughout the day.",
          BlogTags = new List<BlogTag>()
        },
        new Blog
        {
          Id = new Guid("178FB57C-508C-4877-80F3-3607856CF786"),
          StaffId = staffs[3].Id,
          Title = "DIY Skincare: Natural Ingredients for a Radiant Glow",
          Content =
            "Discover how to incorporate natural, kitchen-based ingredients into your skincare routine. This post shares easy DIY recipes and tips for achieving a healthy, radiant glow.",
          BlogTags = new List<BlogTag>()
        },
        new Blog
        {
          Id = new Guid("5E421C87-9AA8-4CC0-A03F-C0F658609241"),
          StaffId = staffs[3].Id,
          Title = "Seasonal Skincare: Adjusting Your Routine for Summer",
          Content =
            "Summer can be harsh on your skin. Learn how to adapt your skincare routine for hot weather with tips on hydration, sun protection, and lightweight product recommendations.",
          BlogTags = new List<BlogTag>()
        },
        new Blog
        {
          Id = new Guid("53292157-7960-4927-9F81-232A0B566F8E"),
          StaffId = staffs[3].Id,
          Title = "Nighttime Skincare: Repair and Rejuvenate Your Skin",
          Content =
            "Nighttime is the perfect time for skin repair. Discover the best products and practices to help your skin recover from daily stress and rejuvenate while you sleep.",
          BlogTags = new List<BlogTag>()
        }
      };
    }
  }

  public static IEnumerable<BlogTag> BlogTags
  {
    get
    {
      var blogs = Blogs.ToList();
      var tags = Tags.ToList();

      return new List<BlogTag>
      {
        // Blog 1: "Skincare Education: Master the Basics" → Skincare Education
        new BlogTag { BlogId = blogs[0].Id, TagId = tags.First(t => t.Name == "Skincare Education").Id, },
        // Blog 2: "Routine Building & Ingredient Guide for Every Skin Concern" → Routine Building
        new BlogTag { BlogId = blogs[1].Id, TagId = tags.First(t => t.Name == "Routine Building").Id, },
        // Blog 2: "Routine Building & Ingredient Guide for Every Skin Concern" → Ingredient Guide
        new BlogTag { BlogId = blogs[1].Id, TagId = tags.First(t => t.Name == "Ingredient Guide").Id, },
        // Blog 3: "Deep Dive into Active Ingredients: Vitamin C & Retinol" → Ingredient Guide
        new BlogTag { BlogId = blogs[2].Id, TagId = tags.First(t => t.Name == "Ingredient Guide").Id, },
        // Blog 4: "How to Build a Customized Skincare Routine" → Routine Building
        new BlogTag { BlogId = blogs[3].Id, TagId = tags.First(t => t.Name == "Routine Building").Id, },
        // Blog 5: "Managing Acne and Other Skin Concerns" → Skin Concerns
        new BlogTag { BlogId = blogs[4].Id, TagId = tags.First(t => t.Name == "Skin Concerns").Id, },
        // Blog 6: "Expert Advice: Dermatologist Tips for Healthy Skin" → Expert Advice
        new BlogTag { BlogId = blogs[5].Id, TagId = tags.First(t => t.Name == "Expert Advice").Id, },
        // Blog 7: "The Ultimate Guide to Sunscreen Application" → Skincare Education
        new BlogTag { BlogId = blogs[6].Id, TagId = tags.First(t => t.Name == "Skincare Education").Id, },
        // Blog 8: "DIY Skincare: Natural Ingredients for a Radiant Glow" → Ingredient Guide
        new BlogTag { BlogId = blogs[7].Id, TagId = tags.First(t => t.Name == "Ingredient Guide").Id, },
        // Blog 9: "Seasonal Skincare: Adjusting Your Routine for Summer" → Routine Building
        new BlogTag { BlogId = blogs[8].Id, TagId = tags.First(t => t.Name == "Routine Building").Id, },
        // Blog 10: "Nighttime Skincare: Repair and Rejuvenate Your Skin" → Skincare Education
        new BlogTag { BlogId = blogs[9].Id, TagId = tags.First(t => t.Name == "Skincare Education").Id, }
      };
    }
  }

  public static IEnumerable<Brand> Brands => new List<Brand>
  {
    new Brand
    {
      Id = new Guid("9286DAB7-CCA8-4C04-9D2D-FF1072A1746A"),
      Name = "L'Oreal",
      Description = "Global beauty brand",
      WebsiteUrl = "https://www.loreal.com",
      LogoUrl = "https://example.com/loreal-logo.png"
    },
    new Brand
    {
      Id = new Guid("A6ACE76F-5DA7-4EEB-A909-49B0390F34A2"),
      Name = "Clinique",
      Description = "Fragrance-free skincare products",
      WebsiteUrl = "https://www.clinique.com",
      LogoUrl = "https://example.com/clinique-logo.png"
    },
    new Brand
    {
      Id = new Guid("735B21D1-AB56-45BD-8404-7722112AE8E2"),
      Name = "Neutrogena",
      Description = "Dermatologist-recommended skincare",
      WebsiteUrl = "https://www.neutrogena.com",
      LogoUrl = "https://example.com/neutrogena-logo.png"
    },
    new Brand
    {
      Id = new Guid("B4D858D8-28D6-476A-9BDF-B9265DA3E160"),
      Name = "CeraVe",
      Description = "Skincare with essential ceramides",
      WebsiteUrl = "https://www.cerave.com",
      LogoUrl = "https://example.com/cerave-logo.png"
    },
    new Brand
    {
      Id = new Guid("0F7ABD8F-0905-4EDE-A573-C46B6EF86267"),
      Name = "The Ordinary",
      Description = "Clinical formulations with integrity",
      WebsiteUrl = "https://www.theordinary.com",
      LogoUrl = "https://example.com/theordinary-logo.png"
    }
  };

  public static IEnumerable<Cart> Carts
  {
    get
    {
      var customers = Users.ToList();
      return new List<Cart>
      {
        new Cart
        {
          Id = new Guid("A5D8471E-7C24-48D9-8233-CD598E6DD1C3"), CustomerId = customers[5].Id
        }
      };
    }
  }

  public static IEnumerable<CartItem> CartItems
  {
    get
    {
      var carts = Carts.ToList();
      var cosmetics = Cosmetics.ToList();
      return new List<CartItem>
      {
        new CartItem { CartId = carts[0].Id, CosmeticId = cosmetics[0].Id, Quantity = 2 },
        new CartItem { CartId = carts[0].Id, CosmeticId = cosmetics[1].Id, Quantity = 1 }
      };
    }
  }

  public static IEnumerable<Category> Categories => new List<Category>
  {
    new Category
    {
      Id = new Guid("0419D5D4-1C6A-4F2D-ACFA-D3E5687C5A73"),
      Name = "Radiance",
      Description = "Enhance your natural glow and brighten your complexion."
    },
    new Category
    {
      Id = new Guid("0CDF8B34-6D34-4669-9757-10C68A69ECAA"),
      Name = "Rejuvenation",
      Description = "Age-defying and skin renewal solutions."
    },
    new Category
    {
      Id = new Guid("222AFB3A-879D-4FB1-AEB4-BCD4EECE09C4"),
      Name = "Purity",
      Description = "Deep cleansing and detoxifying products."
    },
    new Category
    {
      Id = new Guid("ECBE8150-1FF8-4DE8-B13F-44E007E2487D"),
      Name = "Hydration",
      Description = "Products to lock in moisture and nourish your skin."
    },
    new Category
    {
      Id = new Guid("E187FB64-B583-4BF6-A9F7-34C8DCE1CDCF"),
      Name = "Balance",
      Description = "Formulations to maintain skin pH and soothe irritation."
    },
    new Category
    {
      Id = new Guid("16C105C4-D38D-46A1-98CA-48852724DA8F"),
      Name = "Protection",
      Description = "Guard your skin against environmental stressors."
    },
    new Category
    {
      Id = new Guid("67D4B89F-17EE-4458-9678-6454E89547DD"),
      Name = "Specialized Care",
      Description = "Targeted solutions for specific skin concerns."
    },
    new Category
    {
      Id = new Guid("D5E75588-1D27-4FF4-8DDA-6220DF581268"),
      Name = "Masking",
      Description = "Intensive treatments delivered via masks."
    }
  };

  public static IEnumerable<CosmeticType> CosmeticTypes => new List<CosmeticType>
  {
    new CosmeticType
    {
      Id = new Guid("AC635372-0D15-44F7-BF5F-7496C4F16051"),
      Name = "Cleansers",
      Description = "Products that clean the skin."
    },
    new CosmeticType
    {
      Id = new Guid("119B5F44-626F-4831-8978-388A8B2AD23C"),
      Name = "Exfoliators",
      Description = "Products to remove dead skin cells."
    },
    new CosmeticType
    {
      Id = new Guid("A0F0DF4B-07C2-4984-A914-184479DB31B0"),
      Name = "Toners",
      Description = "Products to balance and refresh the skin."
    },
    new CosmeticType
    {
      Id = new Guid("22D4240D-9674-4E6C-B0B7-72F84CA010B8"),
      Name = "Serums",
      Description = "Concentrated treatments for skin."
    },
    new CosmeticType
    {
      Id = new Guid("4CCDED92-BB02-4B8E-9119-95F59628C5D2"),
      Name = "Moisturizers",
      Description = "Products to hydrate and nourish the skin."
    },
    new CosmeticType
    {
      Id = new Guid("79406AF7-46EE-49B4-9D4C-6812A2C396A0"),
      Name = "Eye Creams",
      Description = "Specialized creams for the eye area."
    },
    new CosmeticType
    {
      Id = new Guid("79BD1C90-FAF6-4A08-9893-DBA2552FEE68"),
      Name = "Sunscreens",
      Description = "Products that protect the skin from UV rays."
    },
    new CosmeticType
    {
      Id = new Guid("C6C92631-D452-4C16-A065-3FA93CE750D8"),
      Name = "Lip Care Products",
      Description = "Products to care for and enhance the lips."
    },
    new CosmeticType
    {
      Id = new Guid("C4FDAAE7-B369-4842-B31B-37EAA3072534"),
      Name = "Face Masks",
      Description = "Treatments that refresh and rejuvenate the skin."
    }
  };

  public static IEnumerable<Cosmetic> Cosmetics
  {
    get
    {
      var brands = Brands.ToList();
      var skinTypes = SkinTypes.ToList();
      var cosmeticTypes = CosmeticTypes.ToList();
      var stores = Stores.ToList(); // Get the list of stores to assign their IDs

      var guids = new List<Guid>
      {
        // OSPW
        new Guid("1C30BE5B-CFC7-4381-B4B7-93360CB0EA92"), // Cleanser
        new Guid("D2209B3D-3BDA-4CEC-9420-F324C0FB1806"), // Moisturizer
        new Guid("0C1F6D40-B57A-4FFE-A251-B1534F5AF92F"), // Sunscreen
        new Guid("DECE73A3-3173-454F-A6DF-C917DB5AE0AB"), // Retinoid

        // OSPT
        new Guid("F0D5778B-8BBF-4361-8A6B-51CFCDE06C95"), // Cleanser
        new Guid("18AB23BC-C809-4444-BF27-804211B80CF4"), // Moisturizer
        new Guid("6DB12906-F485-4CAA-B94F-8B5AAA159802"), // Sunscreen
        new Guid("5AB4563F-F656-4749-AD55-F6D2738A7F45"), // Retinoid

        // OSNW
        new Guid("DF8BAFBF-D97E-467D-9746-DDB4A91F8110"), // Cleanser
        new Guid("6DAFC963-C66C-455A-BBBB-2FCFD48DAAA9"), // Moisturizer
        new Guid("AE58E772-2CE9-4602-BA23-EE5B93BDFAE2"), // Sunscreen
        new Guid("E5324EA1-8457-4B57-813E-1B49E3A3E433"), // Retinoid

        // OSNT
        new Guid("B9BF6004-46FC-43BF-88BA-8B3940A6F2D9"), // Cleanser
        new Guid("993A2310-561F-44BF-86F5-BF380CCB8BBE"), // Moisturizer
        new Guid("195F1C1A-CA18-469E-A700-4F9E855321F3"), // Sunscreen
        new Guid("BE6CDC48-09E0-46B6-9F42-311C1791D3E4"), // Retinoid

        // ORPW
        new Guid("84822923-98C6-4D6E-A3CC-7725DC151F5F"), // Cleanser
        new Guid("25BFD41D-5FA6-4A82-B71B-5DC402D82796"), // Moisturizer
        new Guid("53B2CB5A-34B2-4E22-B737-76727D147C2F"), // Sunscreen
        new Guid("BD93A34B-82B2-42CA-9ED3-D09739E2AF92"), // Retinoid

        // ORPT
        new Guid("4D4E6C6B-0AC5-405C-9E82-EAF6C1781FCC"), // Cleanser
        new Guid("81221932-5A62-47AC-A379-9161FFB67CDC"), // Moisturizer
        new Guid("42626649-AF62-4462-9035-C7CA0B882694"), // Sunscreen
        new Guid("BF4C9042-FD7D-4542-8D74-6851935F82FC"), // Retinoid

        // ORNW
        new Guid("FF30734D-F86F-42C2-B0A6-8708DC938DA4"), // Cleanser
        new Guid("6A349A6F-85A7-4314-879D-60D35F240A67"), // Moisturizer
        new Guid("C2DE6B9F-1865-4C1C-85D3-80C701012E75"), // Sunscreen
        new Guid("D01A2F07-5150-4723-854F-29D1BF54ADD9"), // Retinoid

        // ORNT
        new Guid("C421147E-1A16-405C-95DF-2823CEFDA46D"), // Cleanser
        new Guid("750CDD46-5D90-4098-9AA1-82246267D5E7"), // Moisturizer
        new Guid("27B0690D-DDA4-42D6-8AA8-735C3BCA0F82"), // Sunscreen
        new Guid("9CA1DA4E-36BE-4C98-9BF1-D9AFB0A7B616"), // Retinoid

        // DSPW
        new Guid("4E253EB7-DE4F-4508-BE8A-68FE1C84F916"), // Cleanser
        new Guid("80534929-F3FC-46BD-A10D-E9E81CA14B26"), // Moisturizer
        new Guid("1DCFCD24-3F7A-43DB-9377-35507C843638"), // Sunscreen
        new Guid("20046EB9-BBF9-46FA-820A-C6910B32952E"), // Retinoid

        // DSPT
        new Guid("F240AC97-D59B-4A06-8866-19C7701D0CEC"), // Cleanser
        new Guid("2E7E5E13-B6A5-47E5-8B21-95C9250806B9"), // Moisturizer
        new Guid("77872A5D-D393-43A3-B8F4-466CE0EB2091"), // Sunscreen
        new Guid("EA9AAB90-8C31-4C9F-B29B-A0BA75490190"), // Retinoid

        // DSNW
        new Guid("C778C577-DB36-4CA2-8505-551ED2EBC3FB"), // Cleanser
        new Guid("D52F1DEA-19A6-42FB-98FF-1030072A4FC4"), // Moisturizer
        new Guid("B30AF52C-49A4-4D1E-864A-653A7C8ECB51"), // Sunscreen
        new Guid("718435D8-16F8-49BB-A4AF-925310E4DD60"), // Retinoid

        // DSNT
        new Guid("D344B3D3-BFA4-4E78-A31F-8E5602C5ED54"), // Cleanser
        new Guid("2844790D-D680-4764-BFA4-3408EEDD22F4"), // Moisturizer
        new Guid("14FC4977-44D2-47A2-8010-8C39183C5CED"), // Sunscreen
        new Guid("EEAED16D-7E26-4FCD-9227-E87A3923C138"), // Retinoid

        // DRPW
        new Guid("DDCDB352-CC59-439A-8AC5-E01069947368"), // Cleanser
        new Guid("166014F8-23B6-4D42-99AB-9E8773D8EB51"), // Moisturizer
        new Guid("947ED855-52C8-4406-B433-FCFE5AFEBE67"), // Sunscreen
        new Guid("2E4A9F5A-FAB8-41F1-A7E8-7D118C6DF4C9"), // Retinoid

        // DRPT
        new Guid("346B8E78-0249-48F9-AD3C-E25E424E3EFB"), // Cleanser
        new Guid("18DD17D0-E133-49B2-B52A-1CA584096C75"), // Moisturizer
        new Guid("1590DE13-65D5-484A-B7F8-DDF5084AB6FC"), // Sunscreen
        new Guid("B35907DC-6F89-4292-BFB3-2600FBBC17E0"), // Retinoid

        // DRNW
        new Guid("1CF889F1-BA1A-47A2-AD29-EE22AEAE74DB"), // Cleanser
        new Guid("31463C5F-243E-404B-B043-75E5F45B9B02"), // Moisturizer
        new Guid("C508E6D1-E475-4742-B8C9-38DE462275B5"), // Sunscreen
        new Guid("0E22A2D5-D2C3-445A-8039-7920E508E90E"), // Retinoid

        // DRNT
        new Guid("A75C3A77-DADF-4B22-9333-C750B0259A3D"), // Cleanser
        new Guid("6B6F4F10-A1BB-4603-8A4B-6207D78BC17C"), // Moisturizer
        new Guid("B86F428E-5641-4D92-9D78-14592EFA5B8B"), // Sunscreen
        new Guid("EE853A4D-F553-483B-885F-97F4D404BDCE") // Retinoid
      };

      var rand = new Random();

      var cosmetics = new List<Cosmetic>();
      int guidIndex = 0;
      // We'll use Brand[0] for cleanser and moisturizer, and Brand[1] for sunscreen and retinoid.
      foreach (var skin in skinTypes)
      {
        // Randomize brand index for each product type
        int cleanserBrandIndex = rand.Next(brands.Count);
        int moisturizerBrandIndex = rand.Next(brands.Count);
        int sunscreenBrandIndex = rand.Next(brands.Count);
        int retinoidBrandIndex = rand.Next(brands.Count);

        // Cleanser for this skin type - Linked to the first store
        cosmetics.Add(new Cosmetic
        {
          Id = guids[guidIndex++],
          Name = $"{brands[cleanserBrandIndex].Name} {skin.Name} Cleanser",
          BrandId = brands[cleanserBrandIndex].Id,
          SkinTypeId = skin.Id,
          StoreId = stores[0].Id, // Link to the first store
          CosmeticTypeId = cosmeticTypes.First(ct => ct.Name == "Cleansers").Id,
          Gender = true,
          Notice = "A gentle cleanser to prepare the skin.",
          Ingredients = "Water, Mild Surfactants, Herbal Extracts",
          MainUsage = "Cleansing and prepping the skin",
          Texture = "Gel",
          Origin = "USA",
          Instructions = "Apply to wet skin, massage gently, then rinse.",
          Weight = rand.Next(50, 500),
          Length = rand.Next(5, 20),
          Width = rand.Next(5, 20),
          Height = rand.Next(5, 20),
          VolumeUnit = (VolumeUnit)0
        });

        // Moisturizer for this skin type - Linked to the second store
        cosmetics.Add(new Cosmetic
        {
          Id = guids[guidIndex++],
          Name = $"{brands[moisturizerBrandIndex].Name} {skin.Name} Moisturizer",
          BrandId = brands[moisturizerBrandIndex].Id,
          SkinTypeId = skin.Id,
          StoreId = stores[1].Id, // Link to the second store
          CosmeticTypeId = cosmeticTypes.First(ct => ct.Name == "Moisturizers").Id,
          Gender = true,
          Notice = "Hydrates and nourishes the skin.",
          Ingredients = "Hyaluronic Acid, Glycerin, Ceramides",
          MainUsage = "Moisturizing and protection",
          Texture = "Cream",
          Origin = "France",
          Instructions = "Apply to face after cleansing.",
          Weight = rand.Next(50, 500),
          Length = rand.Next(5, 20),
          Width = rand.Next(5, 20),
          Height = rand.Next(5, 20),
          VolumeUnit = (VolumeUnit)0
        });

        // Sunscreen for this skin type - Linked to the third store
        cosmetics.Add(new Cosmetic
        {
          Id = guids[guidIndex++],
          Name = $"{brands[sunscreenBrandIndex].Name} {skin.Name} Sunscreen",
          BrandId = brands[sunscreenBrandIndex].Id,
          SkinTypeId = skin.Id,
          StoreId = stores[2].Id, // Link to the third store
          CosmeticTypeId = cosmeticTypes.First(ct => ct.Name == "Sunscreens").Id,
          Gender = true,
          Notice = "Provides broad-spectrum protection.",
          Ingredients = "Zinc Oxide, Titanium Dioxide",
          MainUsage = "Sun protection",
          Texture = "Lotion",
          Origin = "USA",
          Instructions = "Apply generously 15 minutes before sun exposure.",
          Weight = rand.Next(50, 500),
          Length = rand.Next(5, 20),
          Width = rand.Next(5, 20),
          Height = rand.Next(5, 20),
          VolumeUnit = (VolumeUnit)0
        });

        // Retinoid (using the Serums type) for this skin type - No store assigned
        cosmetics.Add(new Cosmetic
        {
          Id = guids[guidIndex++],
          Name = $"{brands[retinoidBrandIndex].Name} {skin.Name} Anti-Aging Retinoid",
          BrandId = brands[retinoidBrandIndex].Id,
          SkinTypeId = skin.Id,
          StoreId = null, // No store assigned
          CosmeticTypeId = cosmeticTypes.First(ct => ct.Name == "Serums").Id,
          Gender = true,
          Notice = "Helps reduce wrinkles and improve skin texture.",
          Ingredients = "Retinol, Hyaluronic Acid",
          MainUsage = "Anti-aging treatment",
          Texture = "Cream",
          Origin = "Italy",
          Instructions = "Apply a pea-sized amount to cleansed skin at night.",
          Weight = rand.Next(50, 500),
          Length = rand.Next(5, 20),
          Width = rand.Next(5, 20),
          Height = rand.Next(5, 20),
          VolumeUnit = (VolumeUnit)0
        });
      }

      // Add the extra cosmetics with some linked to stores
      cosmetics.AddRange(new List<Cosmetic>
      {
        new Cosmetic
        {
          Id = new Guid("A14E2C76-662E-4FA4-A9A9-81EE4512D441"),
          Name = $"{brands[0].Name} Hydrating Face Cream",
          BrandId = brands[0].Id,
          SkinTypeId = skinTypes[0].Id,
          StoreId = stores[0].Id, // Link to Flagship Store
          CosmeticTypeId = cosmeticTypes[0].Id,
          Gender = true,
          Notice = "Apply twice daily for best results.",
          Ingredients = "Water, Hyaluronic Acid, Glycerin",
          MainUsage = "Moisturizing and hydrating",
          Texture = "Cream",
          Origin = "France",
          Instructions = "Apply on a cleansed face in the morning and at night.",
          Weight = rand.Next(50, 500),
          Length = rand.Next(5, 20),
          Width = rand.Next(5, 20),
          Height = rand.Next(5, 20),
          VolumeUnit = (VolumeUnit)0
        },
        new Cosmetic
        {
          Id = new Guid("70DEC3C1-B8FB-4BCB-AA27-B04B49E5D626"),
          Name = $"{brands[2].Name} Gentle Facial Cleanser",
          BrandId = brands[2].Id,
          SkinTypeId = skinTypes[1].Id,
          StoreId = stores[1].Id, // Link to Crescent Mall Store
          CosmeticTypeId = cosmeticTypes[1].Id,
          Gender = true,
          Notice = "Suitable for daily use.",
          Ingredients = "Salicylic Acid, Chamomile Extract",
          MainUsage = "Cleansing and gentle exfoliation",
          Texture = "Gel",
          Origin = "USA",
          Instructions = "Massage onto wet skin and rinse thoroughly.",
          Weight = rand.Next(50, 500),
          Length = rand.Next(5, 20),
          Width = rand.Next(5, 20),
          Height = rand.Next(5, 20),
          VolumeUnit = (VolumeUnit)0
        },
        new Cosmetic
        {
          Id = new Guid("1D3E5EC9-6FEC-450E-8283-D44D349EDBBF"),
          Name = $"{brands[3].Name} Revitalizing Serum",
          BrandId = brands[3].Id,
          SkinTypeId = skinTypes[1].Id,
          StoreId = null, // No store
          CosmeticTypeId = cosmeticTypes.First(ct => ct.Name == "Serums").Id,
          Gender = true,
          Notice = "Apply a few drops after cleansing.",
          Ingredients = "Hyaluronic Acid, Vitamin C, Peptides",
          MainUsage = "Revitalizing and rejuvenating the skin",
          Texture = "Light Gel",
          Origin = "France",
          Instructions = "Apply 2-3 drops on cleansed skin, morning and night.",
          Weight = rand.Next(50, 500),
          Length = rand.Next(5, 20),
          Width = rand.Next(5, 20),
          Height = rand.Next(5, 20),
          VolumeUnit = (VolumeUnit)0
        }
      });
      return cosmetics;
    }
  }

  public static IEnumerable<CosmeticPrice> CosmeticPrices
  {
    get
    {
      var cosmeticPrices = new List<CosmeticPrice>();
      var rand = new Random();
      foreach (var cosmetic in Cosmetics)
      {
        cosmeticPrices.Add(
          new CosmeticPrice
          {
            Id = Guid.NewGuid(),
            CosmeticId = cosmetic.Id,
            EventId = Events.ElementAt(rand.Next(0, 4)).Id,
            StartDate = new DateTime(2025, 01, 01),
            OriginalPrice = rand.Next(1, 40) * 100000
          }
        );
      }

      return cosmeticPrices;
    }
  }

  public static IEnumerable<Event> Events
  {
    get
    {
      var guids = new List<Guid>()
      {
        new Guid("d4a353a0-3e30-4e01-8fac-9baaf7a45dc7"),
        new Guid("8664fa11-1a47-48b2-aabd-3062345d930d"),
        new Guid("5a4455e5-c39e-4b75-b64d-660366deabf2"),
        new Guid("edabede4-1fac-46ba-b6d2-6d1b06d0096c"),
        new Guid("57934496-4d7c-4e21-aaf6-f42c023033ae")
      };

      var events = new List<Event>() { 
        new Event() {
        Id = guids[0],
        Name = "Black Friday",
        Description = "Last friday of November",
        DiscountPercentage = 30
        } ,
        
        new Event()
        {
          Id = guids[1],
          Name = "New Year Sale",
          Description = "Discount for new years",
          DiscountPercentage = 15
        },
        new Event()
        {
          Id = guids[2],
          Name = "World skin health day",
          Description = "Discount world skin health day on 8th July",
          DiscountPercentage = 20 
        },
        new Event()
        {
          Id = guids[3],
          Name = "Flash Sale",
          Description = "Quick discount",
          DiscountPercentage = 35
        },
        new Event()
        {
          Id = guids[4],
          Name = "None",
          Description = "None",
          DiscountPercentage = 0
        }
      };

      return events;
    }
  }

  public static IEnumerable<CosmeticImage> CosmeticImages
  {
    get
    {
      var cosmetics = Cosmetics.ToList();
      return new List<CosmeticImage>
      {
        new CosmeticImage
        {
          Id = new Guid("74D96977-165F-428B-8DE8-5488D6427355"),
          CosmeticId = cosmetics[0].Id,
          ImageUrl = "https://example.com/hydrating-face-cream.png"
        },
        new CosmeticImage
        {
          Id = new Guid("76AB322C-CD7C-4862-ADA1-15AE2AE78E84"),
          CosmeticId = cosmetics[1].Id,
          ImageUrl = "https://example.com/gentle-facial-cleanser.png"
        }
      };
    }
  }

  public static IEnumerable<CosmeticSubCategory> CosmeticSubCategories
  {
    get
    {
      // Assume these static properties return your seeded data.
      var cosmetics = Cosmetics.ToList();
      var subCategories = SubCategories.ToList();

      var mappings = new List<CosmeticSubCategory>();

      foreach (var cosmetic in cosmetics)
      {
        string cosmeticName = cosmetic.Name.ToLower();

        // Map cleansers to "Deep Cleansing" under Purity.
        if (cosmeticName.Contains("cleanser"))
        {
          var sub = subCategories.FirstOrDefault(s => s.Name == "Deep Cleansing");
          if (sub != null)
          {
            mappings.Add(new CosmeticSubCategory { CosmeticId = cosmetic.Id, SubCategoryId = sub.Id, });
          }
        }
        // Map moisturizers and face creams to "Nourishing Creams" under Hydration.
        else if (cosmeticName.Contains("moisturizer") || cosmeticName.Contains("face cream"))
        {
          var sub = subCategories.FirstOrDefault(s => s.Name == "Nourishing Creams");
          if (sub != null)
          {
            mappings.Add(new CosmeticSubCategory { CosmeticId = cosmetic.Id, SubCategoryId = sub.Id, });
          }
        }
        // Map sunscreens to "SPF Essentials" under Protection.
        else if (cosmeticName.Contains("sunscreen"))
        {
          var sub = subCategories.FirstOrDefault(s => s.Name == "SPF Essentials");
          if (sub != null)
          {
            mappings.Add(new CosmeticSubCategory { CosmeticId = cosmetic.Id, SubCategoryId = sub.Id, });
          }
        }
        // Map retinoids to "Age-Defying Treatments" under Rejuvenation.
        else if (cosmeticName.Contains("retinoid"))
        {
          var sub = subCategories.FirstOrDefault(s => s.Name == "Age-Defying Treatments");
          if (sub != null)
          {
            mappings.Add(new CosmeticSubCategory { CosmeticId = cosmetic.Id, SubCategoryId = sub.Id, });
          }
        }
        // Map toners to "Soothing Solutions" under Balance.
        else if (cosmeticName.Contains("toner"))
        {
          var sub = subCategories.FirstOrDefault(s => s.Name == "Soothing Solutions");
          if (sub != null)
          {
            mappings.Add(new CosmeticSubCategory { CosmeticId = cosmetic.Id, SubCategoryId = sub.Id, });
          }
        }
        // Map scrubs to "Detox & Clarify" under Purity.
        else if (cosmeticName.Contains("scrub"))
        {
          var sub = subCategories.FirstOrDefault(s => s.Name == "Detox & Clarify");
          if (sub != null)
          {
            mappings.Add(new CosmeticSubCategory { CosmeticId = cosmetic.Id, SubCategoryId = sub.Id, });
          }
        }
        // Map eye creams to "Illuminators" under Radiance.
        else if (cosmeticName.Contains("eye cream"))
        {
          var sub = subCategories.FirstOrDefault(s => s.Name == "Illuminators");
          if (sub != null)
          {
            mappings.Add(new CosmeticSubCategory { CosmeticId = cosmetic.Id, SubCategoryId = sub.Id, });
          }
        }
        // Map lip balms to "Delicate Area Care" under Specialized Care.
        else if (cosmeticName.Contains("lip balm"))
        {
          var sub = subCategories.FirstOrDefault(s => s.Name == "Delicate Area Care");
          if (sub != null)
          {
            mappings.Add(new CosmeticSubCategory { CosmeticId = cosmetic.Id, SubCategoryId = sub.Id, });
          }
        }
        // Map face masks to "Sheet Masks" under Masking.
        else if (cosmeticName.Contains("face mask"))
        {
          var sub = subCategories.FirstOrDefault(s => s.Name == "Sheet Masks");
          if (sub != null)
          {
            mappings.Add(new CosmeticSubCategory { CosmeticId = cosmetic.Id, SubCategoryId = sub.Id, });
          }
        }
      }

      return mappings;
    }
  }

  public static IEnumerable<CompanyInformation> CompanyInfos => new List<CompanyInformation>
  {
    new CompanyInformation
    {
      Id = new Guid("DD7C5D83-2B34-4943-87EC-95D965C13FF9"),
      Name = "TechCorp",
      Description = "A leading technology company.",
      LogoUrl = "https://example.com/logo.png",
      Email = "contact@techcorp.com",
      PhoneNumber = "+123456789",
      Address = "123 Tech Street, Silicon Valley, CA"
    }
  };

  public static IEnumerable<Coupon> Coupons => new List<Coupon>
  {
    new Coupon
    {
      Id = new Guid("6E8F40E3-7A19-4A41-B3F8-4DFF00FD8C21"),
      Code = "DISCOUNT10",
      StartDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), // Explicitly set to UTC
      EndDate = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc), // Explicitly set to UTC
      DiscountAmount = 10.00,
      UsageLimit = 100
    }
  };

  public static IEnumerable<FAQ> FAQs => new List<FAQ>
  {
    new FAQ
    {
      Id = new Guid("2B4AE24C-EED6-4EBC-AFD9-5DA8E22037F3"),
      Question = "How do I reset my password?",
      Answer = "You can reset your password in the settings page."
    },
    new FAQ
    {
      Id = new Guid("21AE2A57-761C-4A36-B5FF-62488BF8F4FB"),
      Question = "Where can I contact support?",
      Answer = "You can contact support at support@example.com."
    }
  };

  public static IEnumerable<Feedback> Feedbacks
  {
    get
    {
      var users = Users.ToList();
      var cosmetics = Cosmetics.ToList();
      return new List<Feedback>
      {
        new Feedback
        {
          Id = new Guid("16784D61-99F6-4242-879D-AE16C088AE0D"),
          CosmeticId = cosmetics[0].Id,
          CustomerId = users[5].Id,
          Content = "This cleanser left my skin feeling incredibly soft and refreshed.",
          Rating = 4.5m
        },
        new Feedback
        {
          Id = new Guid("476FB2C8-476B-4426-B025-87BEE1AE00C6"),
          CosmeticId = cosmetics[1].Id,
          CustomerId = users[5].Id,
          Content = "The serum really brightened my complexion. Highly recommend!",
          Rating = 5.0m
        },
        new Feedback
        {
          Id = new Guid("F7D08F8A-4650-4320-89DE-FD790D992BF0"),
          CosmeticId = cosmetics[0].Id,
          CustomerId = users[5].Id,
          Content = "Decent product, but didn't meet all my expectations.",
          Rating = 3.5m
        }
      };
    }
  }

  public static IEnumerable<Order> Orders
  {
    get
    {
      var customer = Users.ToList();
      var coupon = Coupons.First();
      return new List<Order>
      {
        //new Order
        //{
        //  Id = new Guid("F72B5E3B-C443-4920-9D03-BB774721F70A"),
        //  CustomerId = customer[5].Id,
        //  CouponId = coupon.Id,
        //  SubTotal = 89.97m,
        //  TotalPrice = 79.97m,
        //  OrderDate = DateTime.UtcNow,
        //  ShippingAddress = "123 Main Street, City, Country",
        //  BillingAddress = "123 Main Street, City, Country",
        //  TrackingNumber = "TRACK123",
        //  DeliveryDate = DateTime.UtcNow.AddDays(5),
        //  Status = "Processing"
        //}
      };
    }
  }

  public static IEnumerable<OrderItem> OrderItems
  {
    get
    {
      //var order = Orders.First();
      var cosmetics = Cosmetics.ToList();
      return new List<OrderItem>
      {
        //new OrderItem
        //{
        //  OrderId = order.Id,
        //  CosmeticId = cosmetics[0].Id,
        //  Quantity = 2
        //},
        //new OrderItem
        //{
        //  OrderId = order.Id,
        //  CosmeticId = cosmetics[1].Id,
        //  Quantity = 1
        //}
      };
    }
  }

  public static IEnumerable<Policy> Policies => new List<Policy>
  {
    new Policy
    {
      Id = new Guid("70B1B2A8-8361-468C-A602-04E1A815495E"),
      Title = "Privacy Policy",
      Content = "This is the privacy policy content."
    },
    new Policy
    {
      Id = new Guid("EC19577C-8D19-4497-801A-11B66E55666D"),
      Title = "Terms of Service",
      Content = "These are the terms of service."
    }
  };

  public static IEnumerable<QuestionType> QuestionsTypes => new List<QuestionType>
  {
    new QuestionType() { Id = new Guid("AAAC424F-B3E1-4E13-8E3F-F06C7AD1A141"), Name = "SingleChoice", },
    new QuestionType() { Id = new Guid("7F9D392B-1673-4227-A3A4-ACDCD6E1ACF5"), Name = "MultipleChoice", },
  };

  public static Quiz Quiz
  {
    get
    {
      var questionTypes = QuestionsTypes.ToList();

      return new Quiz
      {
        Id = new Guid("4330503E-4488-4272-80D5-34B25D5A8677"),
        Title = "Which of the 16 Baumann Skin Types® Are You?",
        Description =
          "Take the 3 minute skin type quiz now and build a skin care routine with medical grade skin care brands. You will be amazed by how great your skin will look!",
        TargetAgeFrom = 18,
        TargetAgeTo = 65,
        Questions = new List<Question>
        {
          // Oiliness (score > 0 is oily)
          new Question
          {
            Id = new Guid("6273C0C4-FDF1-416C-A5ED-4E89C071BF90"),
            QuizId = new Guid("A184BF67-B94A-4787-A78D-9C35EE913451"),
            Title = "Assess your skin moisturization needs",
            Description =
              "Please check what is true about how often you must use a moisturizer for your skin to feel hydrated.",
            Instruction = "(Multiple answers allowed)",
            Section = "Oiliness",
            QuestionTypeId = questionTypes[1].Id,
            QuestionOptions = new List<QuestionOption>
            {
              new QuestionOption
              {
                Id = new Guid("A3D27E0B-E737-4B7E-92C9-529A3978B52A"),
                QuestionId = new Guid("1E619B1A-E08B-4735-95A1-B785ACBCE5D6"),
                Content = "I can use any soap to wash my face without developing dryness.",
                Score = 2,
              },
              new QuestionOption
              {
                Id = new Guid("DADD010D-E2CB-47C1-B3DD-6B9E7554EAA6"),
                QuestionId = new Guid("9C333E2A-0D24-4A5C-9874-38BF49796ABE"),
                Content = "I never or only occasionally apply a moisturizer.",
                Score = 1,
              },
              new QuestionOption
              {
                Id = new Guid("763ED597-5BB7-49AC-9826-7FCD63CE24EE"),
                QuestionId = new Guid("3EBFBEBD-EA4A-4388-B8F9-09C594B1B481"),
                Content = "I do not apply any products to my facial skin after cleansing.",
                Score = 0,
              },
              new QuestionOption
              {
                Id = new Guid("FFE5E5DC-9786-4EB1-817E-1DEA8FFEE8AB"),
                QuestionId = new Guid("B987D596-124D-48B5-ACF4-F50966E17B25"),
                Content = "I apply a moisturizer to my face once a day.",
                Score = -1,
              },
              new QuestionOption
              {
                Id = new Guid("00347781-0390-402A-86AC-223839172276"),
                QuestionId = new Guid("4970FD6B-D7D7-4F51-B812-EE679E5A3F8A"),
                Content = "I apply a moisturizer to my face twice a day.",
                Score = -2,
              }
            }
          },
          new Question
          {
            Id = new Guid("678D1C72-9888-44F1-B2C3-E793568ACA9A"),
            QuizId = new Guid("E7F686E9-57EE-4CBD-807D-D61D5A97F9B3"),
            Title = "Assess your skin's sebum production",
            Description = "Please check what is true about your facial skin.",
            Instruction = "(Multiple answers allowed)",
            Section = "Oiliness",
            QuestionTypeId = questionTypes[1].Id,
            QuestionOptions = new List<QuestionOption>
            {
              new QuestionOption
              {
                Id = new Guid("4C9A9592-4BAD-4C39-94A8-C72693956E5F"),
                QuestionId = new Guid("9D6B8570-23E9-4E68-983E-9BC49605AEDA"),
                Content = "My facial skin is rough or dry.",
                Score = -2,
              },
              new QuestionOption
              {
                Id = new Guid("530552D6-F817-4E42-AF24-6F86070E1BA5"),
                QuestionId = new Guid("655D6860-0ABE-4C61-8A5B-366F159ACBD9"),
                Content = "My facial skin is oily in some areas.",
                Score = 1,
              },
              new QuestionOption
              {
                Id = new Guid("BE4749FF-6BF4-450C-8486-345A2D248135"),
                QuestionId = new Guid("66E7EA6F-3C55-4B0F-B56F-38692A833DDD"),
                Content = "My face is very oily.",
                Score = 2,
              },
              new QuestionOption
              {
                Id = new Guid("FCE0D8BE-FE60-4DCA-B672-F2D0655E4509"),
                QuestionId = new Guid("53401F2D-A377-4546-8339-A991E2328ADA"),
                Content = "My face is uncomfortable if I do not use a moisturizer.",
                Score = -2,
              },
              new QuestionOption
              {
                Id = new Guid("8CC40510-4BF6-4E2D-B24D-AE22F9EA20F2"),
                QuestionId = new Guid("ED08CFF3-0D6E-4679-B562-3167B769D7CF"),
                Content = "I like the feel of heavy creams and/or oil on my skin.",
                Score = -2,
              },
            }
          },
          // Sensitivity (score > 0 is sensitive)
          new Question
          {
            Id = new Guid("BBE970DB-0609-469F-9B0A-6A736A5F1F47"),
            QuizId = new Guid("C24472F3-21D7-4331-9F96-BBA2CE114C99"),
            Title = "Assess your skin's underlying inflammation",
            Description = "Check the following that you have had in the last 4 weeks",
            Instruction = "(Multiple answers allowed)",
            Section = "Sensitivity",
            QuestionTypeId = questionTypes[1].Id,
            QuestionOptions = new List<QuestionOption>
            {
              new QuestionOption
              {
                Id = new Guid("738CE3A0-0E2E-4913-8CE6-5E942A1C2CDB"),
                QuestionId = new Guid("0370A764-93FB-4458-993F-F92D242A876E"),
                Content = "Acne (pimples)",
                Score = 1,
              },
              new QuestionOption
              {
                Id = new Guid("FFE74667-33BD-4ACC-9904-5274C5985005"),
                QuestionId = new Guid("5A2D5237-F1BA-4508-BB19-F30C89EC05A9"),
                Content = "Facial redness and/or flushing",
                Score = 1,
              },
              new QuestionOption
              {
                Id = new Guid("D7F8E751-8C8C-403D-BEFE-9C048176AC8B"),
                QuestionId = new Guid("8136C736-F111-40C3-992D-B11AEDB84863"),
                Content = "Stinging or burning",
                Score = 1,
              },
              new QuestionOption
              {
                Id = new Guid("D3724236-B2C3-4374-8C53-C312DF4F71E8"),
                QuestionId = new Guid("18F0763B-3F7A-4FCB-B43D-D6D19B53E842"),
                Content = "A rash with itching, scaling and redness",
                Score = 1,
              },
              new QuestionOption
              {
                Id = new Guid("104B5D59-7634-4EA3-AFDB-26C9E267DE6B"),
                QuestionId = new Guid("8DF17F50-9702-4897-A145-AD1EB2D42C2D"),
                Content = "Irritation from shaving the face",
                Score = 1,
              },
            }
          },
          // Pigmentation (score > 0 is uneven)
          new Question
          {
            Id = new Guid("91B1AD98-3DC6-490D-AFFA-B1C3483157F5"),
            QuizId = new Guid("995DF52D-8C2E-472C-BE1D-4DF12ED1F416"),
            Title = "Do you want to lighten dark spots on your skin?",
            Description = "Do you want skin lighteners in your skin care products to treat hyper pigmentation?",
            Instruction = "(Choose one answer)",
            Section = "Pigmentation",
            QuestionTypeId = questionTypes[0].Id,
            QuestionOptions = new List<QuestionOption>
            {
              new QuestionOption
              {
                Id = new Guid("EB06FC0F-BA5A-45B3-8779-92553D53EC0F"),
                QuestionId = new Guid("2E7CD29A-DD8C-4112-9353-F32542B4D34E"),
                Content = "My skin pigment is uneven AND I want to lighten darker areas on my face",
                Score = 1,
              },
              new QuestionOption
              {
                Id = new Guid("666FFAEA-0A76-46F5-9544-6B45BF0641E6"),
                QuestionId = new Guid("CF714571-92F5-4594-84D7-FE7CFF500D25"),
                Content = "My skin pigment is even AND I have no dark spots or darker areas",
                Score = -1,
              },
              new QuestionOption
              {
                Id = new Guid("2911A59A-D1A6-46CA-896B-6F1B944F005B"),
                QuestionId = new Guid("0EE02D34-41EA-493C-B53E-D2C19BB64E18"),
                Content = "I have freckles or dark spots AND I do not want to remove",
                Score = 0,
              },
            }
          },
          // Aging / Wrinkles (score > 0 is aging)
          new Question
          {
            Id = new Guid("B6C91837-8EB4-4558-98B2-79B9165DF3C2"),
            QuizId = new Guid("64E06944-1EA1-4770-B73E-217D50460DC5"),
            Title = "Lifestyle habits",
            Description = "Check all that apply to you.",
            Instruction = "(Multiple answers allowed)",
            Section = "Aging",
            QuestionTypeId = questionTypes[1].Id,
            QuestionOptions = new List<QuestionOption>
            {
              new QuestionOption
              {
                Id = new Guid("B9367A38-C8E9-482E-A962-37D6F9ED4BCE"),
                QuestionId = new Guid("E6CFD235-84F9-4A34-A3B7-D90A940AE619"),
                Content = "I have smoked over 50 cigarettes or cigars in my life.",
                Score = -2
              },
              new QuestionOption
              {
                Id = new Guid("9C070F77-3962-45CF-A47D-537E087863FA"),
                QuestionId = new Guid("0D767C75-8295-45A6-8DB2-122325EEC395"),
                Content = "I am exposed to second hand smoke on a weekly basis.",
                Score = -1,
              },
              new QuestionOption
              {
                Id = new Guid("DF98BB35-3350-44BE-A1EA-12BD4557A7EC"),
                QuestionId = new Guid("19FE99F7-324F-4ABB-BBE3-435668C5D015"),
                Content = "I currently smoke cigarettes or cigars",
                Score = -2,
              },
              new QuestionOption
              {
                Id = new Guid("28CA8B91-34B5-4AFD-A2F7-8AC3E5EE71B4"),
                QuestionId = new Guid("7CE65305-8848-4987-9D86-B748A643C4A0"),
                Content = "I often get less than 7 hours of sleep a night.",
                Score = 2,
              },
              new QuestionOption
              {
                Id = new Guid("B2CE0FF3-FE77-442B-B77D-3445433F38F9"),
                QuestionId = new Guid("D4E68CEA-4204-473C-B200-D1085A937BCC"),
                Content = "I feel stress at least 2 hours a day.",
                Score = 2,
              },
              new QuestionOption
              {
                Id = new Guid("242A9DAB-CDFE-498D-B6A2-D4FD3CC3BDE2"),
                QuestionId = new Guid("70127183-BBF5-4B30-891B-552552E0D3DE"),
                Content = "Are you exposed to pollution or bad air quality more than 3 times a week?",
                Score = 2,
              },
              new QuestionOption
              {
                Id = new Guid("D5F4C05E-59D3-4DC7-B73C-D5971A890BEE"),
                QuestionId = new Guid("5745A1C7-4E8C-4E40-8473-DEFC062C4555"),
                Content = "I eat sugary foods over 3 times a week.",
                Score = 1,
              },
              new QuestionOption
              {
                Id = new Guid("03991A2E-D887-4967-B3B8-8D1E78AFAC53"),
                QuestionId = new Guid("878035E2-E137-4BF2-ABEF-EFEDF41E9D2F"),
                Content = "I exercise less than 3 hours a week.",
                Score = 1,
              },
              new QuestionOption
              {
                Id = new Guid("825D7C7D-C003-484E-81B0-B866BD019DB0"),
                QuestionId = new Guid("7D50D71D-2715-4FF5-AD13-EE82F21D1771"),
                Content = "I do not eat fruit or vegetables every day.",
                Score = 1,
              },
            }
          }
        }
      };
    }
  }

  public static IEnumerable<Routine> Routines
  {
    get
    {
      var skinTypes = SkinTypes.ToList();
      var routines = new List<Routine>();

      var guids = new List<Guid>
      {
        // OSPW
        new Guid("AE1B0472-F179-4602-81C4-F119EED0244D"), // Morning
        new Guid("DDBF879B-9B48-453A-AED6-6CA155DB2D91"), // Evening

        // OSPT
        new Guid("C8278F1D-5776-45EE-A81C-F6A87AE772AC"), // Morning
        new Guid("5DD2714D-1B90-4324-AE2B-17F805D71B98"), // Evening

        // OSNW
        new Guid("17519DCD-4E0E-4339-B284-2D886072B881"), // Morning
        new Guid("A2BA97BB-AAB1-42B6-A216-668E2D96F8D3"), // Evening

        // OSNT
        new Guid("29C70348-314A-4FF5-8889-95E5BBB02C99"), // Morning
        new Guid("9A3C73ED-4A26-4440-BBA9-85AA61E33FD1"), // Evening

        // ORPW
        new Guid("4094E874-57FA-4CED-ADCD-12B3AA97E2FF"), // Morning
        new Guid("6F36F894-1657-4466-B30C-398BF8BCB609"), // Evening

        // ORPT
        new Guid("A75C3A77-DADF-4B22-9333-C750B0259A3D"), // Morning
        new Guid("6B6F4F10-A1BB-4603-8A4B-6207D78BC17C"), // Evening

        // ORNW
        new Guid("E5E5F974-5693-4058-B008-9CD42B9CF7D6"), // Morning
        new Guid("68C9ABA8-515A-4151-AF0F-10827031134A"), // Evening

        // ORNT
        new Guid("AE80C7AF-686B-401E-A45D-9B31849BFEE8"), // Morning
        new Guid("C4FE9C8A-6D77-4D2D-A077-E5389CDAD605"), // Evening

        // DSPW
        new Guid("A801EF46-CDEF-401D-8559-E35D3AA0F2D5"), // Morning
        new Guid("A1CDF96C-5475-4B6D-B958-D2B6816FFA8A"), // Evening

        // DSPT
        new Guid("CC03E4E5-66DA-4369-8FFD-0E648AB0DC6E"), // Morning
        new Guid("0A440730-CAED-4F25-91F7-4707B94E1FAF"), // Evening

        // DSNW
        new Guid("F2D35C17-00E5-41DF-82A3-F084DA5B91C6"), // Morning
        new Guid("5E9B6B12-2A76-456E-8D2F-C4BEBEBADC60"), // Evening

        // DSNT
        new Guid("23CE213D-19DB-4A43-9AEA-D4A3B8783C1B"), // Morning
        new Guid("E4CAE7FF-4CB8-49B9-A6D7-AD3DB847965B"), // Evening

        // DRPW
        new Guid("1F371142-ED1E-4094-8A5F-F88652A8FD6F"), // Morning
        new Guid("83510517-C5AE-47A9-8F2D-904084A1FB61"), // Evening

        // DRPT
        new Guid("3FEDF110-C654-4FA2-9C01-6F16A58C1A9A"), // Morning
        new Guid("A29B83A2-6110-450C-83B3-8EC18AC25A70"), // Evening

        // DRNW
        new Guid("7520F072-8928-4776-AEA4-826D8431E28C"), // Morning
        new Guid("B058C77A-6497-4A84-9096-87ADEAC41A1B"), // Evening

        // DRNT
        new Guid("F5A8A3FD-A6E6-4607-AD51-14476DD9E9AD"), // Morning
        new Guid("079F2254-A4C1-4FC7-839E-2398DAF1B118"), // Evening
      };
      int guidIndex = 0;

      foreach (var skin in skinTypes)
      {
        // Morning Routine for this skin type
        var morningRoutine = new Routine
        {
          Id = guids[guidIndex++], SkinTypeId = skin.Id, Title = $"{skin.Name} Morning Routine", Period = "Morning",
        };

        // Evening Routine for this skin type
        var eveningRoutine = new Routine
        {
          Id = guids[guidIndex++], SkinTypeId = skin.Id, Title = $"{skin.Name} Evening Routine", Period = "Evening",
        };

        // Add to the list
        routines.Add(morningRoutine);
        routines.Add(eveningRoutine);
      }

      return routines;
    }
  }

  public static IEnumerable<RoutineStep> RoutineSteps
  {
    get
    {
      var routines = Routines.ToList();
      var cosmetics = Cosmetics.ToList();
      var steps = new List<RoutineStep>();

      var guids = new List<Guid>()
      {
        new Guid("EBE1E2D8-892F-42CA-B1C3-9AB1D37D5DD6"),
        new Guid("5C8A1410-DC91-4D64-A801-BFF9223895C0"),
        new Guid("A3DCB51A-9AB1-4F9C-A888-6B852AB7093D"),
        new Guid("1B10376B-D880-4DFE-A0DB-430B7E7E204A"),
        new Guid("91674B5D-A1E6-4CB8-B72B-7C7962DFA9F5"),
        new Guid("2126EF18-5FD7-4F24-92F8-28777CCF44D5"),
        new Guid("03A980B8-6279-4A92-9515-598151BDA586"),
        new Guid("CE341DC2-0034-4FAC-A1F8-2CDE6A5A067D"),
        new Guid("4D9C741B-E5FE-4549-A6AD-FF3D56F0A808"),
        new Guid("FEA73374-1994-4323-BD35-9A2FF438CA6E"),
        new Guid("9E76622F-8B7D-42F5-868C-89A5A54AE63F"),
        new Guid("B7511B29-116B-4F6C-BB50-BD3D348B73F3"),
        new Guid("80D36224-463D-46A2-902D-637362A7915C"),
        new Guid("33EC32FA-E2A8-40C4-8D26-6BE329D0A140"),
        new Guid("8D52145E-B707-4681-8376-7BE648BD664E"),
        new Guid("B501BAF1-1452-49CD-AE08-9F363CC2A594"),
        new Guid("780FCAB0-9157-4D50-97F0-E73FF76028E8"),
        new Guid("237FD265-960D-4ECA-8575-C025842C5437"),
        new Guid("95D5FA81-D889-4A43-BDB3-17D99F2D5DC7"),
        new Guid("4A38A900-8D3E-489E-814C-802C670F0BA3"),
        new Guid("8543F178-FCC2-477A-9AB8-A6DD8300B13A"),
        new Guid("679F0120-D758-46BD-9D72-080658A4EFA6"),
        new Guid("D1953F4F-B0F8-47B0-9DF1-52283AEF2FA8"),
        new Guid("72D6C9FC-BE8A-4AAB-9D68-6E26B6C5CCA5"),
        new Guid("C360C765-1305-4063-831D-A3859563E3BD"),
        new Guid("6998FBBE-B736-4B3E-BA70-C7D5A96C863C"),
        new Guid("B334EDCC-19A5-4E9A-ADB6-083821F593DE"),
        new Guid("03BE768B-5989-4050-BCFC-C80FFE8C4447"),
        new Guid("589F3724-DEA5-4BDD-B01C-19A9CA2C7DB6"),
        new Guid("578B466E-4346-434C-A97E-F1B80130441C"),
        new Guid("7E999A1A-2D59-472B-BFA0-CF6086103B64"),
        new Guid("A90B5740-EABD-4124-800B-E28065D9DE06"),
        new Guid("99FA0490-B67C-4D00-AE49-6B21624F7153"),
        new Guid("3CD758F7-1170-4514-AE21-A847D8CE2557"),
        new Guid("BEAA14B1-E560-40E3-B69E-9BF9050072EA"),
        new Guid("B12822A3-860E-4D2A-A8CB-3E51CF7A6E52"),
        new Guid("AF128D8B-3026-49B1-BB65-23DE4BF6F8B8"),
        new Guid("8911F7CA-2ECA-4DD9-8754-63DCEC67B4D2"),
        new Guid("071B8888-8059-47CD-9376-866CE705CE61"),
        new Guid("0E837974-A472-4EF5-8A97-CD39C188ADB3"),
        new Guid("0C76812A-DDED-4EA2-B711-9B005631385D"),
        new Guid("FB08B60A-8924-4A2A-8690-61C135398F20"),
        new Guid("258DB312-DB68-4972-B274-57A44E78553E"),
        new Guid("AE75BBBB-629A-448D-A599-D53622766499"),
        new Guid("C35CD8BF-392F-4FCC-BD99-CB366FDA317A"),
        new Guid("4CC67F2B-1A6D-4F90-B0DE-7CA4BADF2600"),
        new Guid("15DD0C9F-4FBA-4F71-A7DD-E15B44FBF923"),
        new Guid("AB4A6D35-B286-4D74-BF60-4E08959FC18B"),
        new Guid("EA3224BA-7E09-491E-B4E1-655301641182"),
        new Guid("0C9947ED-588C-434F-AB57-9AB61F901644"),
        new Guid("09CDF083-E677-445A-B7CA-EDEAF4AFE718"),
        new Guid("2F3A67D1-3189-4B6D-A348-70515FA73121"),
        new Guid("632EBD5E-B494-4A49-8C47-31B536FAA113"),
        new Guid("3FCA76D7-0D5F-46EA-BB90-CCB791DE1AE3"),
        new Guid("D45019B6-9056-49BF-8413-7956299BF077"),
        new Guid("70D4E333-5342-42A7-B35D-B1642325A1A3"),
        new Guid("408E665F-C5BA-419B-89FE-6DF807E61863"),
        new Guid("8B901E9C-F163-4890-8D76-D1DE47E797BC"),
        new Guid("4D5C9D8B-A474-45EF-BCF5-5CE7D6C88A35"),
        new Guid("747916EB-8D49-45DB-B7F3-1736CD33DDCE"),
        new Guid("C98B2559-011B-4990-8607-C8AFE3D09904"),
        new Guid("5FE52826-FE55-4CF8-AF8A-247F32232874"),
        new Guid("86A8669F-EBF3-4057-90EE-5A0C3F9036AD"),
        new Guid("BE461E93-7712-4170-BDEE-FD73F5701D2D"),
        new Guid("29BDFD68-73C3-406F-BCAD-6AEB35489063"),
        new Guid("92C201F5-DAAA-4064-8B72-DEB01FE3EAF0"),
        new Guid("B90E30EB-6F3B-40CD-B2D6-BA0AB9E1D5D1"),
        new Guid("87344DB2-0D7E-4C56-9702-7FD10B7C0145"),
        new Guid("3F2F9985-B0B3-4772-A4A7-347BA8F87756"),
        new Guid("52B08AB8-BC82-46F2-91BB-C0479CF7DBD2"),
        new Guid("7DE1D870-DA0F-4F5D-819F-FB1EFEFF57EA"),
        new Guid("BEC1A622-C617-4420-9263-E33CF4C619FF"),
        new Guid("6078D1A2-A926-4CC5-87EF-375C2E0F6EEC"),
        new Guid("90CE3D13-6CF9-4441-89FE-D1328AC60DB8"),
        new Guid("D61F21AB-2969-499A-BAE5-00F98BE7E2E1"),
        new Guid("2ED310D1-D847-4ABC-80DF-29AA095E6412"),
        new Guid("F3F8E8BC-C144-49CD-A21D-38A75D51332E"),
        new Guid("AB3DBB18-9B79-495B-AF11-3347171EF201"),
        new Guid("5A45E4BC-7C4B-49AD-9808-BB33846A0E0D"),
        new Guid("05329EFD-367C-4154-96B9-53F9BF0724DE"),
        new Guid("6C904BE0-7C44-43CB-90A1-A70C494B1603"),
        new Guid("21EAEE83-6F3C-4E05-A14B-7A190B1A7F52"),
        new Guid("64698C5A-3AB2-4511-83AC-EA483660AF42"),
        new Guid("9391BEC2-3105-47C1-9925-EEAAA71DF04F"),
        new Guid("2AFA436D-4F52-4B1E-A753-91F9E2CBCB60"),
        new Guid("89C97315-07CC-40BB-A673-E92C38C190E7"),
        new Guid("D928E5D6-D76C-490B-95AA-E11B8287CD22"),
        new Guid("89CB248B-4E09-4D84-9628-A04EDBEAE882"),
        new Guid("D809D9A5-69CF-4B86-8C9D-B8A1B4D9AB61"),
        new Guid("14DA23B7-AEC6-48B5-A2E8-24794B6F4B25"),
        new Guid("1969C7A5-6947-40B4-80A8-18221DF9573E"),
        new Guid("3B0C6A72-CB68-436A-B1AF-3F748E6BE34B"),
        new Guid("FDC3B33C-AC82-4B28-9D7C-895D46D6F182"),
        new Guid("F4801B86-87D3-4DF8-A000-0E561CE2D0EE"),
        new Guid("929AB13D-84A5-4D2A-A802-AA2952691C6C"),
        new Guid("0C5A8EF0-D859-49B8-8B4E-67E865E2972B"),
        new Guid("E950B5BC-814C-4A5D-BF60-9415772EDA52"),
        new Guid("C7C889BC-B1D6-42B4-93E1-910FAAE7D329"),
        new Guid("FE04E7BF-ED42-4CB8-B020-D1C76A116F4F"),
        new Guid("0059F32A-B85A-4B60-96D0-61DAAD0B87F9"),
        new Guid("5ABA4084-3C68-4D02-B8EA-F9A55141B450"),
        new Guid("115F2C56-2E2A-45DF-8AA6-239D4AE44822"),
        new Guid("5CAC0790-1055-4872-BF0B-DFAF65C93C08"),
        new Guid("4D91106B-6A5F-442D-87EC-BD21A322D392"),
        new Guid("00CF1334-2AC3-4028-BDE8-4F4BF2EEC9C6"),
        new Guid("17069818-8480-41D2-BA59-55549ABBF3A6"),
        new Guid("9055748B-C8B2-4E24-869A-D555D9891668"),
        new Guid("B203BAAB-66B5-4644-8A96-429C0061BF40"),
        new Guid("DE31AEDA-FEE5-4527-8053-42D08317DB0F"),
        new Guid("DCA7B2FE-E51F-4A39-8652-0540291D56BB"),
        new Guid("5E8C5AEB-6E40-491A-A9AB-6F31571E5397"),
        new Guid("20584DE0-EC21-40F4-8292-4BCBF2FB95A5"),
        new Guid("332D74C8-455A-44FE-AC55-295148AC2623"),
        new Guid("AC51DE3F-609E-4004-9F91-902FD70D9EFD"),
        new Guid("FBEF2B1C-CA5B-436F-BDA3-3E921CFACAFC"),
        new Guid("5CA9788F-3BC7-4B58-9301-0BB445EE32EC"),
        new Guid("1D13A8A6-A3E1-4F93-A2BF-7CDECF5E75EB"),
        new Guid("784754C2-0097-4173-84DA-F1AC8CC4BA03"),
        new Guid("924F637E-1FEE-4B4C-ACE1-ED2264DA295A"),
        new Guid("3F32756C-1DE1-4C1B-AF4F-C8CC22D04E13"),
        new Guid("EF42F0A5-3D80-4F3C-9804-5CE8B1C24512"),
        new Guid("550E9DA1-BDED-4423-B62E-C6984D389244"),
        new Guid("48AF1863-5C78-4A81-B05E-1B1702AB7B15"),
        new Guid("26BD27DC-38F1-4137-8995-C8EBCF4369F4"),
        new Guid("24136F2D-54FA-4EF0-AE34-9826B8D85D56"),
        new Guid("CCDB483D-C449-4677-AF13-1D4772EF61EA"),
        new Guid("B7D22199-7760-4635-A707-9ACAF2AB679B"),
        new Guid("282B36CD-0CA5-4682-8C8C-E4E9FFD63982"),
        new Guid("11048F34-8044-40B6-B241-DF75E9BEED63"),
        new Guid("2671FA9A-7C22-45C8-A3E2-D50B44A2738F"),
        new Guid("450A6B25-B1DB-4BFA-B161-3B7705855106"),
        new Guid("18DC983C-20E8-4D24-B81A-F0DA159F977B"),
        new Guid("2EE4BB29-8C52-4901-B3E4-F2C9BF42D934"),
        new Guid("611F74D3-58F8-4672-915F-5921DB456DC6"),
        new Guid("FFEAB20D-DCDD-4582-BB82-019DA4F5E718"),
        new Guid("F5192758-A9FA-4467-AFF5-3D641EDB2824"),
        new Guid("6C76AB62-2B65-42A1-8947-688941E5DBD4"),
        new Guid("701BE5D4-7BF0-4DB5-B7A5-DA869B8BB79C"),
        new Guid("3D1BD2AD-4236-43CC-9CB8-527BF8CE5E0C"),
        new Guid("7CD17E51-68D8-40C7-89C4-1B20CD389AE2"),
        new Guid("F36B091E-5589-49D5-9EEC-E6F175F6FD74"),
        new Guid("AAC2501E-C2F9-4D3C-B391-A3D4BDFED0DC"),
        new Guid("FDF13EF7-99E8-449C-94BD-1ADA8C9B82FE"),
        new Guid("12F4D37B-D6EE-477D-A261-BBFFDA7D5E20"),
        new Guid("B9EC3F39-F802-4270-A860-A8C57A549636"),
        new Guid("89D4E457-E48D-43D9-AA88-A541D41AA913"),
        new Guid("2C330691-538A-45C8-A974-9E501198618F"),
        new Guid("D92E08D5-8E79-4FE4-842D-C5B59D813EDB"),
        new Guid("78B19C55-D7F8-4CB6-A5C9-97F53D417B6E"),
        new Guid("1C4C25EA-25C6-43F1-A73C-126685100C70"),
        new Guid("C11C2D3E-7AA0-4E0B-99A3-7E370C715784"),
        new Guid("64F1DF34-C313-414F-895D-45F670816CDF"),
        new Guid("EBA8E12C-0C97-499F-969A-A7E9627C0AA9"),
        new Guid("1A88BB5B-7F6D-4A57-BC3B-A4EC7078686B"),
        new Guid("BE0CE84F-ADBA-489B-98E1-D7B8731F87CC"),
        new Guid("EFE9443F-873D-4F6C-9DF4-46F388A10BD3"),
        new Guid("7438EB2C-40F6-4052-9E19-93860C8DD577"),
        new Guid("E8E47B37-3C99-4D13-A5F1-A0BB9D02B6D6"),
        new Guid("13434667-B642-4BDA-BE2C-23C664A901EA"),
        new Guid("6B3CB178-AB4D-460A-8C64-814C19CA8271"),
        new Guid("0FCCAE59-B2DB-4937-88F3-F04E30BBB6AD"),
        new Guid("3E925CAF-0A79-4D57-A0D7-1BD2CD77B262"),
        new Guid("D6E86B67-F94A-4477-A91C-B415F7050FBA"),
        new Guid("79424585-4D8A-4405-B3E0-F30E0A9E46BE"),
        new Guid("16E0BF85-5EBA-469F-84B6-AD512020D270"),
        new Guid("1A349C45-D5C1-43FE-8D9B-04132BBEC086"),
        new Guid("A202F6D3-B0BB-49CF-AEC1-4DEBB525BC36"),
        new Guid("9FA23C9A-3A89-4B07-96CB-6A1C1F5BAA0C"),
        new Guid("D157A9DA-2B86-42A5-BB93-933D500D76D0"),
        new Guid("99CA0118-0A6B-4D28-A6CA-4F21E3B5F732"),
        new Guid("731E691A-B408-41F2-AB92-4CAB0E4B4B42"),
        new Guid("65F5AAA6-6070-47E4-854C-9937054FD6AF"),
        new Guid("C07C0F40-9503-4AF6-8140-3899148CE3FF"),
        new Guid("C1085A0C-755B-4AC9-A43F-D40A4A8871D9"),
        new Guid("DDDB4698-D08E-4583-A519-6345F62E6A17"),
        new Guid("FBBC5DE6-9CDE-4CA1-BE8A-19336B8D5511"),
        new Guid("BE07EF32-B80B-4C37-BF76-AB91FB7FA4B9"),
        new Guid("44BF728C-91C3-4E27-AEFD-98C76A2CC4DD"),
        new Guid("924A5D17-2963-452A-A793-50BBE05164E8"),
        new Guid("8DD444E0-A229-4FCE-8F18-8D16C56F5D91"),
        new Guid("9EDCFB8B-A7A5-456C-B1DB-E6D6CBBDF586"),
        new Guid("FCD1F255-77B1-4D11-A931-FF10EBBE7366"),
        new Guid("ED07A49E-19C6-4E06-B600-DFBF27737352"),
        new Guid("3D0D5C41-7A80-4F63-B211-01D4FA9383DC"),
        new Guid("9338FE60-0A3E-411C-9AB1-86695FDE4D27"),
        new Guid("69047E14-ADC9-482F-8BBE-C46B74019C4A"),
        new Guid("773BB13F-BFA5-41AF-B597-3DFC8ABE48A6"),
        new Guid("5BBE879F-90C4-4C77-868A-760D65613FF3"),
        new Guid("D4E0C5A2-5F99-4CCA-987E-7B34A3FCBA2A"),
        new Guid("8EB0A324-1A54-4CBA-979A-254632BFBC8F"),
        new Guid("631DDF90-5987-485D-ADAB-17C9561C26FB"),
        new Guid("2F1F2102-57F6-4744-93B0-5A0CAFCAE1D9"),
        new Guid("CFAF1037-0A9D-4CF9-9099-16C9F80D6286"),
        new Guid("62217830-871A-4E15-A157-7DD053422646"),
        new Guid("FC0AD6AA-7D95-4A22-8049-6F5B236320B4"),
        new Guid("EC5B56C2-77A8-4AAE-AB05-45C83A98D60F"),
      };

      var guidIndex = 0;

      foreach (var routine in routines)
      {
        var cleanser = cosmetics.FirstOrDefault(c =>
          c.Name.Contains("cleanser", StringComparison.OrdinalIgnoreCase) && c.SkinTypeId == routine.SkinTypeId);
        var moisturizer = cosmetics.FirstOrDefault(c =>
          c.Name.Contains("moisturizer", StringComparison.OrdinalIgnoreCase) && c.SkinTypeId == routine.SkinTypeId);
        var sunscreen = cosmetics.FirstOrDefault(c =>
          c.Name.Contains("sunscreen", StringComparison.OrdinalIgnoreCase) && c.SkinTypeId == routine.SkinTypeId);
        var retinoid = cosmetics.FirstOrDefault(c =>
          c.Name.Contains("retinoid", StringComparison.OrdinalIgnoreCase) && c.SkinTypeId == routine.SkinTypeId);
        if (routine.Period.Equals("Morning", StringComparison.OrdinalIgnoreCase))
        {
          // Morning Routine Steps
          steps.Add(new RoutineStep
          {
            Id = guids[guidIndex++], RoutineId = routine.Id, CosmeticId = cleanser?.Id ?? Guid.Empty, StepNumber = 1
          });
          steps.Add(new RoutineStep
          {
            Id = guids[guidIndex++],
            RoutineId = routine.Id,
            CosmeticId = moisturizer?.Id ?? Guid.Empty,
            StepNumber = 2
          });
          steps.Add(new RoutineStep
          {
            Id = guids[guidIndex++], RoutineId = routine.Id, CosmeticId = sunscreen?.Id ?? Guid.Empty, StepNumber = 3
          });
        }
        else if (routine.Period.Equals("Evening", StringComparison.OrdinalIgnoreCase))
        {
          // Evening Routine Steps
          steps.Add(new RoutineStep
          {
            Id = guids[guidIndex++], RoutineId = routine.Id, CosmeticId = cleanser?.Id ?? Guid.Empty, StepNumber = 1
          });
          steps.Add(new RoutineStep
          {
            Id = guids[guidIndex++],
            RoutineId = routine.Id,
            CosmeticId = moisturizer?.Id ?? Guid.Empty,
            StepNumber = 2
          });
          steps.Add(new RoutineStep
          {
            Id = guids[guidIndex++], RoutineId = routine.Id, CosmeticId = retinoid?.Id ?? Guid.Empty, StepNumber = 3
          });
        }
      }

      return steps;
    }
  }

  public static IEnumerable<SkinType> SkinTypes => new List<SkinType>
  {
    new SkinType
    {
      Id = new Guid("F73B3596-D3F0-4DA9-BA46-0E75D332658B"),
      Name = "OSPW",
      Description = "Oily, Sensitive, Pigmented, Wrinkle-Prone",
      IsDry = false,
      IsSensitive = true,
      IsUneven = true,
      IsWrinkle = true
    },
    new SkinType
    {
      Id = new Guid("D7BCD97E-BE00-4CFC-83DE-1BCCE52F2CE3"),
      Name = "OSPT",
      Description = "Oily, Sensitive, Pigmented, Tight",
      IsDry = false,
      IsSensitive = true,
      IsUneven = true,
      IsWrinkle = false
    },
    new SkinType
    {
      Id = new Guid("20B72075-FE5F-429E-A75B-2C3DDF705A9C"),
      Name = "OSNW",
      Description = "Oily, Sensitive, Non-Pigmented, Wrinkle-Prone",
      IsDry = false,
      IsSensitive = true,
      IsUneven = false,
      IsWrinkle = true
    },
    new SkinType
    {
      Id = new Guid("E9307F73-A099-4930-8A76-1214F0E006DC"),
      Name = "OSNT",
      Description = "Oily, Sensitive, Non-Pigmented, Tight",
      IsDry = false,
      IsSensitive = true,
      IsUneven = false,
      IsWrinkle = false
    },
    new SkinType
    {
      Id = new Guid("F20DFE0E-3C5B-463A-8C28-7E54B3B62CCB"),
      Name = "ORPW",
      Description = "Oily, Resistant, Pigmented, Wrinkle-Prone",
      IsDry = false,
      IsSensitive = false,
      IsUneven = true,
      IsWrinkle = true
    },
    new SkinType
    {
      Id = new Guid("90700FA6-65DF-4D5D-93CF-1D5A6E9DA668"),
      Name = "ORPT",
      Description = "Oily, Resistant, Pigmented, Tight",
      IsDry = false,
      IsSensitive = false,
      IsUneven = true,
      IsWrinkle = false
    },
    new SkinType
    {
      Id = new Guid("210E8376-E992-4981-ACB8-C396B5DE20A3"),
      Name = "ORNW",
      Description = "Oily, Resistant, Non-Pigmented, Wrinkle-Prone",
      IsDry = false,
      IsSensitive = false,
      IsUneven = false,
      IsWrinkle = true
    },
    new SkinType
    {
      Id = new Guid("331EC094-8847-436B-9C79-A7B2D28F4B42"),
      Name = "ORNT",
      Description = "Oily, Resistant, Non-Pigmented, Tight",
      IsDry = false,
      IsSensitive = false,
      IsUneven = false,
      IsWrinkle = false
    },
    new SkinType
    {
      Id = new Guid("D412BC64-2134-40AD-9556-54F31B87A96E"),
      Name = "DSPW",
      Description = "Dry, Sensitive, Pigmented, Wrinkle-Prone",
      IsDry = true,
      IsSensitive = true,
      IsUneven = true,
      IsWrinkle = true
    },
    new SkinType
    {
      Id = new Guid("FEE794FC-41B9-4741-8774-21116EC8B3D3"),
      Name = "DSPT",
      Description = "Dry, Sensitive, Pigmented, Tight",
      IsDry = true,
      IsSensitive = true,
      IsUneven = true,
      IsWrinkle = false
    },
    new SkinType
    {
      Id = new Guid("63B38920-67FF-43E2-A9DD-95B989D064B4"),
      Name = "DSNW",
      Description = "Dry, Sensitive, Non-Pigmented, Wrinkle-Prone",
      IsDry = true,
      IsSensitive = true,
      IsUneven = false,
      IsWrinkle = true
    },
    new SkinType
    {
      Id = new Guid("7AC046BE-36EC-4DCF-AF8E-88DEFEE5023A"),
      Name = "DSNT",
      Description = "Dry, Sensitive, Non-Pigmented, Tight",
      IsDry = true,
      IsSensitive = true,
      IsUneven = false,
      IsWrinkle = false
    },
    new SkinType
    {
      Id = new Guid("2FC2B67E-D050-422D-BAE2-318A243FADDA"),
      Name = "DRPW",
      Description = "Dry, Resistant, Pigmented, Wrinkle-Prone",
      IsDry = true,
      IsSensitive = false,
      IsUneven = true,
      IsWrinkle = true
    },
    new SkinType
    {
      Id = new Guid("63E671F4-E686-4C46-B4DE-EB9B19319916"),
      Name = "DRPT",
      Description = "Dry, Resistant, Pigmented, Tight",
      IsDry = true,
      IsSensitive = false,
      IsUneven = true,
      IsWrinkle = false
    },
    new SkinType
    {
      Id = new Guid("13310480-17DA-405D-B4B4-ACBAED5A5E7A"),
      Name = "DRNW",
      Description = "Dry, Resistant, Non-Pigmented, Wrinkle-Prone",
      IsDry = true,
      IsSensitive = false,
      IsUneven = false,
      IsWrinkle = true
    },
    new SkinType
    {
      Id = new Guid("F585889E-076E-434A-A47F-AC952736712C"),
      Name = "DRNT",
      Description = "Dry, Resistant, Non-Pigmented, Tight",
      IsDry = true,
      IsSensitive = false,
      IsUneven = false,
      IsWrinkle = false
    }
  };

  public static IEnumerable<SubCategory> SubCategories
  {
    get
    {
      var categories = new List<Category>(Categories);

      return new List<SubCategory>
      {
        // Radiance (Category 1)
        new SubCategory
        {
          Id = new Guid("FA5585BA-D79A-4738-973A-2A46BAAFEF02"),
          CategoryId = categories[0].Id,
          Name = "Glow Boosters",
          Description = "Enhance natural radiance and luminosity."
        },
        new SubCategory
        {
          Id = new Guid("924F9CB9-DA41-4B51-A9E6-3F43B15BF4A6"),
          CategoryId = categories[0].Id,
          Name = "Brightening Formulas",
          Description = "Products formulated to brighten and even out skin tone."
        },
        new SubCategory
        {
          Id = new Guid("9B4F16A4-3163-4744-9399-E9381819283A"),
          CategoryId = categories[0].Id,
          Name = "Illuminators",
          Description = "Subtle enhancers for a lit-from-within glow."
        },

        // Rejuvenation (Category 2)
        new SubCategory
        {
          Id = new Guid("A8EF488E-56C6-46AC-889B-4A0FFBD680AC"),
          CategoryId = categories[1].Id,
          Name = "Age-Defying Treatments",
          Description = "Reduce the signs of aging and smooth wrinkles."
        },
        new SubCategory
        {
          Id = new Guid("9A924542-D844-43C2-88CB-5641D00A22DD"),
          CategoryId = categories[1].Id,
          Name = "Firming Solutions",
          Description = "Products that help to tighten and firm the skin."
        },
        new SubCategory
        {
          Id = new Guid("277169B9-E26C-4EE1-9CBC-B4E326E82EE0"),
          CategoryId = categories[1].Id,
          Name = "Renewal Complex",
          Description = "Promote cell turnover and skin renewal."
        },

        // Purity (Category 3)
        new SubCategory
        {
          Id = new Guid("C7C096CE-8959-494D-8913-4139CA48B160"),
          CategoryId = categories[2].Id,
          Name = "Deep Cleansing",
          Description = "Thorough cleansers to purify the skin."
        },
        new SubCategory
        {
          Id = new Guid("FA60A320-490A-4CCF-8BA8-1386DF682A61"),
          CategoryId = categories[2].Id,
          Name = "Detox & Clarify",
          Description = "Remove impurities and unclog pores."
        },
        new SubCategory
        {
          Id = new Guid("17E228DD-F780-4CCE-8884-DFE530FD5A00"),
          CategoryId = categories[2].Id,
          Name = "Pore Refiners",
          Description = "Minimize pores and smooth skin texture."
        },

        // Hydration (Category 4)
        new SubCategory
        {
          Id = new Guid("7FB4A783-8587-4A18-A0DA-94D40682EEFC"),
          CategoryId = categories[3].Id,
          Name = "Moisture Locks",
          Description = "Seal in hydration for long-lasting moisture."
        },
        new SubCategory
        {
          Id = new Guid("F86FA2D9-B3C1-48DF-828E-4BD6CDD76319"),
          CategoryId = categories[3].Id,
          Name = "Hydrating Essentials",
          Description = "Fundamental products for daily hydration."
        },
        new SubCategory
        {
          Id = new Guid("C6A71C5D-A067-4BB6-8D9C-3CA3F180ADFD"),
          CategoryId = categories[3].Id,
          Name = "Nourishing Creams",
          Description = "Rich creams that deeply nourish the skin."
        },

        // Balance (Category 5)
        new SubCategory
        {
          Id = new Guid("71895946-0B7C-40F9-97E0-9195D7BDC5E2"),
          CategoryId = categories[4].Id,
          Name = "pH Balancing",
          Description = "Products to maintain the skin's natural pH."
        },
        new SubCategory
        {
          Id = new Guid("9AD8FBB8-DB09-4842-95B3-590F3728ED42"),
          CategoryId = categories[4].Id,
          Name = "Soothing Solutions",
          Description = "Calm and reduce irritation for balanced skin."
        },

        // Protection (Category 6)
        new SubCategory
        {
          Id = new Guid("C4A28A94-14DA-452D-96EB-1B01D4892C84"),
          CategoryId = categories[5].Id,
          Name = "Environmental Shields",
          Description = "Defend skin against pollution and external stressors."
        },
        new SubCategory
        {
          Id = new Guid("39783437-38B8-480A-B9A5-4814E9920C36"),
          CategoryId = categories[5].Id,
          Name = "SPF Essentials",
          Description = "Sunscreens and UV protective formulations."
        },

        // Specialized Care (Category 7)
        new SubCategory
        {
          Id = new Guid("7B314245-8C44-4E8D-B763-155291ED57C8"),
          CategoryId = categories[6].Id,
          Name = "Targeted Remedies",
          Description = "Specific solutions for defined skin issues."
        },
        new SubCategory
        {
          Id = new Guid("646E7856-BF86-46FB-A71A-799FDAC30F82"),
          CategoryId = categories[6].Id,
          Name = "Delicate Area Care",
          Description = "Gentle products for sensitive areas like eyes and lips."
        },

        // Masking (Category 8)
        new SubCategory
        {
          Id = new Guid("93894942-ADB7-4D19-ABBF-1AA6620B5BCB"),
          CategoryId = categories[7].Id,
          Name = "Sheet Masks",
          Description = "Single-use masks for an instant boost."
        },
        new SubCategory
        {
          Id = new Guid("DADC109E-FA2E-4BE0-A5E2-29FC46F54826"),
          CategoryId = categories[7].Id,
          Name = "Overnight Masks",
          Description = "Leave-on treatments for intensive overnight care."
        }
      };
    }
  }

  public static IEnumerable<Tag> Tags => new List<Tag>
  {
    new Tag
    {
      Id = new Guid("4C592F29-29A6-49F7-B9FF-1EC76D36826A"),
      Name = "Skincare Education",
      Description = "Posts focused on skincare tips, tutorials, and educational content."
    },
    new Tag
    {
      Id = new Guid("555B8613-1FC3-465C-8020-2C1E6E8D4668"),
      Name = "Ingredient Guide",
      Description = "Posts exploring skincare ingredients, their benefits, and usage."
    },
    new Tag
    {
      Id = new Guid("0DD9E52B-443F-4512-B44C-BC79E90B36CE"),
      Name = "Routine Building",
      Description = "Posts dedicated to building and optimizing skincare routines."
    },
    new Tag
    {
      Id = new Guid("4ACB26EF-6046-4396-A16A-473756BD2EB0"),
      Name = "Skin Concerns",
      Description = "Posts addressing common skin concerns and potential solutions."
    },
    new Tag
    {
      Id = new Guid("EED76A4E-A40A-4472-841A-A351D01EA588"),
      Name = "Expert Advice",
      Description = "Posts offering professional insights and expert advice on skincare."
    }
  };

  public static IEnumerable<Testimonial> Testimonials => new List<Testimonial>
  {
    new Testimonial
    {
      Id = new Guid("A7804842-3B9E-47A0-9D0D-B464203D5C62"),
      CustomerId = new Guid("A1E34814-7E85-4858-B0AB-2D9E2DA27D99"),
      Content = "This product changed my life! I can't recommend it enough.",
      Rating = 5
    },
    new Testimonial
    {
      Id = new Guid("80E56A47-A71D-4B9C-AD7B-061FADAE4EA2"),
      CustomerId = new Guid("A1E34814-7E85-4858-B0AB-2D9E2DA27D99"),
      Content = "Great quality and fast shipping. Very satisfied.",
      Rating = 4
    },
    new Testimonial
    {
      Id = new Guid("A073408F-52CD-4C3C-87FF-5EDDC32C0AB8"),
      CustomerId = new Guid("A1E34814-7E85-4858-B0AB-2D9E2DA27D99"),
      Content = "Not what I expected, but still a decent product.",
      Rating = 3
    }
  };
}
using Microsoft.AspNetCore.Identity;
using PickleballClub.Models;
using Microsoft.EntityFrameworkCore;

namespace PickleballClub.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            string[] roleNames = { "Admin", "Member", "Guest", "Referee" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed Users and Members
            var currentMemberCount = await context.Members.CountAsync();
            if (currentMemberCount < 8)
            {
                // Create Admin
                var adminEmail = "admin@pcm.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                    await userManager.CreateAsync(adminUser, "Admin@123");
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

                if (!context.Members.Any(m => m.IdentityUserId == adminUser.Id))
                {
                    context.Members.Add(new Member
                    {
                        IdentityUserId = adminUser.Id,
                        FullName = "Quản trị viên Hệ thống",
                        DOB = new DateTime(1990, 5, 15),
                        PhoneNumber = "0901234567",
                        JoinDate = DateTime.Now.AddMonths(-6),
                        RankLevel = 5.0,
                        Status = "Active"
                    });
                }

                // Create 8 Members
                string[] names = { "Nguyễn Văn An", "Trần Thị Bình", "Lê Văn Cường", "Phạm Minh Đức", "Hoàng Thu Thảo", "Vũ Huy Hoàng", "Đặng Quang Minh", "Bùi Hồng Ngọc" };
                for (int i = 0; i < names.Length; i++)
                {
                    var email = $"member{i + 1}@pcm.com";
                    var user = await userManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
                        await userManager.CreateAsync(user, "Member@123");
                        await userManager.AddToRoleAsync(user, "Member");
                    }

                    if (!context.Members.Any(m => m.IdentityUserId == user.Id))
                    {
                        context.Members.Add(new Member
                        {
                            IdentityUserId = user.Id,
                            FullName = names[i],
                            DOB = new DateTime(1985 + i, 1, 1),
                            PhoneNumber = $"098765432{i}",
                            JoinDate = DateTime.Now.AddDays(-30 * (i + 1)),
                            RankLevel = 2.5 + (i * 0.2),
                            Status = "Active"
                        });
                    }
                }
                await context.SaveChangesAsync();
            }

            var members = await context.Members.ToListAsync();

            // Seed Courts
            if (!context.Courts.Any())
            {
                context.Courts.AddRange(
                    new Court { Name = "Sân 1", IsActive = true, Description = "Sân trung tâm, mái che" },
                    new Court { Name = "Sân 2", IsActive = true, Description = "Sân tập luyện" }
                );
                await context.SaveChangesAsync();
            }

            var courts = await context.Courts.ToListAsync();

            // Seed Challenges
            if (await context.Challenges.CountAsync() < 7)
            {
                // Clear old low quality challenges if any
                var oldChallenges = await context.Challenges.Where(c => c.Title.Length < 5).ToListAsync();
                if (oldChallenges.Any()) context.Challenges.RemoveRange(oldChallenges);

                for (int i = 1; i <= 8; i++)
                {
                    context.Challenges.Add(new Challenge
                    {
                        Title = $"Giải Pickleball Giao lưu #{i}",
                        Description = $"Thử thách trình độ nâng cao dành cho thành viên CLB. Giải thưởng hấp dẫn và cơ hội cọ xát đỉnh cao.",
                        Type = i % 2 == 0 ? ChallengeType.MiniGame : ChallengeType.Duel,
                        GameMode = i % 2 == 0 ? ChallengeGameMode.TeamBattle : ChallengeGameMode.None,
                        RewardDescription = i % 2 == 0 ? "Vợt Selkirk + 1.000.000đ" : "Bộ phụ kiện Joola + 500.000đ",
                        EntryFee = 150000,
                        Status = i < 6 ? ChallengeStatus.Open : ChallengeStatus.Finished,
                        CreatedDate = DateTime.Now.AddDays(-10),
                        ModifiedDate = DateTime.Now.AddDays(-10)
                    });
                }
                await context.SaveChangesAsync();
            }

            // Seed Participants
            var challenges = await context.Challenges.ToListAsync();
            foreach (var c in challenges)
            {
                if (!context.Participants.Any(p => p.ChallengeId == c.Id))
                {
                    for (int j = 0; j < Math.Min(3, members.Count); j++)
                    {
                        context.Participants.Add(new Participant 
                        { 
                            ChallengeId = c.Id, 
                            MemberId = members[j].Id,
                            Team = j % 2 == 0 ? ParticipantTeam.TeamA : ParticipantTeam.TeamB,
                            Status = ParticipantStatus.Confirmed,
                            EntryFeePaid = true,
                            EntryFeeAmount = c.EntryFee
                        });
                    }
                }
            }
            await context.SaveChangesAsync();

            // Seed Bookings
            if (await context.Bookings.CountAsync() < 5)
            {
                for (int i = 0; i < 8; i++)
                {
                    context.Bookings.Add(new Booking
                    {
                        MemberId = members[i % members.Count].Id,
                        CourtId = courts[i % courts.Count].Id,
                        StartTime = DateTime.Now.AddDays(i + 1).Date.AddHours(17),
                        EndTime = DateTime.Now.AddDays(i + 1).Date.AddHours(19),
                        Notes = "Đặt sân tập luyện chuyên sâu",
                        Status = BookingStatus.Confirmed,
                        CreatedDate = DateTime.Now.AddDays(-i)
                    });
                }
                await context.SaveChangesAsync();
            }

            // Seed Matches
            if (await context.Matches.CountAsync() < 5)
            {
                for (int i = 0; i < 8; i++)
                {
                    context.Matches.Add(new Match
                    {
                        MatchDate = DateTime.Now.AddDays(-i - 1),
                        Format = i % 2 == 0 ? MatchFormat.Doubles : MatchFormat.Singles,
                        IsRanked = true,
                        Team1_Player1Id = members[0].Id,
                        Team1_Player2Id = members[1].Id,
                        Team2_Player1Id = members[2].Id,
                        Team2_Player2Id = members[3].Id,
                        WinningSide = i % 2 == 0 ? WinningSide.Team1 : WinningSide.Team2,
                        IsApproved = true
                    });
                }
                await context.SaveChangesAsync();
            }

            // Seed News
            if (await context.News.CountAsync() < 5)
            {
                string[] newsTitles = { "Khai xuân Pickleball 2026", "Giải vô địch CLB mở rộng", "Tuyển thêm thành viên nòng cốt", "Lịch bảo trì sân bãi tháng 2", "Chương trình ưu đãi ngày lễ", "Cập nhật bảng xếp hạng DUPR tháng 1" };
                foreach (var title in newsTitles)
                {
                    context.News.Add(new News { Title = title, Content = $"Nội dung thông tin chi tiết về {title} đã được hội đồng quản trị CLB phê duyệt và ban hành.", PostedDate = DateTime.Now.AddDays(-5) });
                }
                await context.SaveChangesAsync();
            }

            // Seed Treasury
            if (!context.TransactionCategories.Any())
            {
                context.TransactionCategories.AddRange(
                    new TransactionCategory { CategoryName = "Phí hội viên", Type = TransactionType.Income },
                    new TransactionCategory { CategoryName = "Quỹ Kèo đấu", Type = TransactionType.Income },
                    new TransactionCategory { CategoryName = "Tiền phạt", Type = TransactionType.Income },
                    new TransactionCategory { CategoryName = "Tiền sân", Type = TransactionType.Expense },
                    new TransactionCategory { CategoryName = "Mua bóng", Type = TransactionType.Expense },
                    new TransactionCategory { CategoryName = "Tổ chức sự kiện", Type = TransactionType.Expense },
                    new TransactionCategory { CategoryName = "Nước & Phụ kiện", Type = TransactionType.Expense }
                );
                await context.SaveChangesAsync();
            }

            var storedCats = await context.TransactionCategories.ToListAsync();
            if (await context.Transactions.CountAsync() < 10)
            {
                for (int i = 0; i < 15; i++)
                {
                    var cat = storedCats[i % storedCats.Count];
                    context.Transactions.Add(new Transaction
                    {
                        CategoryId = cat.Id,
                        Amount = cat.Type == TransactionType.Income ? (200000 * (i + 1)) : (-100000 * (i + 1)),
                        TransactionDate = DateTime.Now.AddDays(-i - 1),
                        Description = $"Giao dịch mẫu {i + 1}: {cat.CategoryName}"
                    });
                }
                await context.SaveChangesAsync();
            }
        }
    }
}

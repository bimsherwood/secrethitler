using SecretHitler;

namespace SecretHitlerTest;

[TestClass]
public class RoleAssignerTests {

    private string[] SamplePlayers = new[]{
        "Adolf",
        "Ben",
        "Connor",
        "Denise",
        "Erin",
        "Freddy",
        "Garry",
        "Himler",
        "Indiana",
        "Jack",
        "Kim"
    };

    private List<Player> CreatePlayers(int count) => SamplePlayers.Take(count).Select(o => new Player(o)).ToList();

    [TestMethod]
    public void RoleAssignerForFive() {
        var rand = new Random(1234);
        var assigner = new RoleAssigner(rand);
        var game = new GameState(CreatePlayers(5));
        assigner.AssignRoles(game);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsHitler), 1);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsConfederate), 1);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsLiberal), 3);
    }

    [TestMethod]
    public void RoleAssignerForSix() {
        var rand = new Random(1234);
        var assigner = new RoleAssigner(rand);
        var game = new GameState(CreatePlayers(6));
        assigner.AssignRoles(game);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsHitler), 1);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsConfederate), 1);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsLiberal), 4);
    }

    [TestMethod]
    public void RoleAssignerForSeven() {
        var rand = new Random(1234);
        var assigner = new RoleAssigner(rand);
        var game = new GameState(CreatePlayers(7));
        assigner.AssignRoles(game);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsHitler), 1);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsConfederate), 2);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsLiberal), 4);
    }

    [TestMethod]
    public void RoleAssignerForEight() {
        var rand = new Random(1234);
        var assigner = new RoleAssigner(rand);
        var game = new GameState(CreatePlayers(8));
        assigner.AssignRoles(game);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsHitler), 1);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsConfederate), 2);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsLiberal), 5);
    }

    [TestMethod]
    public void RoleAssignerForNein() {
        var rand = new Random(1234);
        var assigner = new RoleAssigner(rand);
        var game = new GameState(CreatePlayers(9));
        assigner.AssignRoles(game);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsHitler), 1);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsConfederate), 3);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsLiberal), 5);
    }

    [TestMethod]
    public void RoleAssignerForTen() {
        var rand = new Random(1234);
        var assigner = new RoleAssigner(rand);
        var game = new GameState(CreatePlayers(10));
        assigner.AssignRoles(game);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsHitler), 1);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsConfederate), 3);
        Assert.AreEqual(game.Roles.Count(o => o.Value.IsLiberal), 6);
    }

    [TestMethod]
    public void RoleAssignerForTooFew() {
        var rand = new Random(1234);
        var assigner = new RoleAssigner(rand);
        var game = new GameState(CreatePlayers(4));
        Assert.ThrowsException<InvalidOperationException>(() => assigner.AssignRoles(game));
    }
    
    [TestMethod]
    public void RoleAssignerForTooMany() {
        var rand = new Random(1234);
        var assigner = new RoleAssigner(rand);
        var game = new GameState(CreatePlayers(11));
        Assert.ThrowsException<InvalidOperationException>(() => assigner.AssignRoles(game));
    }

}
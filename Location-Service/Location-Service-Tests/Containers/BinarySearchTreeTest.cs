using System.Reflection;
using Location_Service.Containers;

namespace Location_Service_Tests.Containers;

public class BinarySearchTreeTest
{
    [Test]
    public void ShouldInsertCorrectly()
    {
        // Arrange
        var tree = new BinarySearchTree<int, string>();
        
        // Act
        Insert(tree);
        
        // Assert
        var root = GetRoot(tree);
        
        Assert.That(root, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(root!.Key, Is.EqualTo(10));
            Assert.That(root!.Left!.Key, Is.EqualTo(5));
            Assert.That(root!.Left!.Left!.Key, Is.EqualTo(2));
            Assert.That(root!.Left!.Right!.Key, Is.EqualTo(7));
            Assert.That(root!.Right!.Key, Is.EqualTo(15));
        });
    }

    [Test]
    [TestCase(10, "a")]
    [TestCase(5, "b")]
    [TestCase(2, "c")]
    [TestCase(7, "d")]
    [TestCase(15, "e")]
    public void ShouldReturnValue(int key, string value)
    {
        // Arrange
        var tree = new BinarySearchTree<int, string>();
        Insert(tree);
        
        // Act
        string? returnValue = tree.Get(key);
        
        // Assert
        Assert.That(returnValue, Is.Not.Null);
        Assert.That(returnValue, Is.EqualTo(value));
    }

    [Test]
    public void ShouldReturnSize()
    {
        // Arrange
        var tree = new BinarySearchTree<int, string>();
        Insert(tree);
        
        // Act
        int size = tree.Size();
        
        // Assert
        Assert.That(size, Is.EqualTo(5));
    }

    [Test]
    public void ShouldReturnTrueIfKeyExists()
    {
        // Arrange
        var tree = new BinarySearchTree<int, string>();
        Insert(tree);
        
        // Act
        bool contains = tree.Contains(2);
        
        // Assert
        Assert.That(contains, Is.True);
    }
    
    [Test]
    public void ShouldReturnFalseIfKeyDoesNotExists()
    {
        // Arrange
        var tree = new BinarySearchTree<int, string>();
        Insert(tree);
        
        // Act
        bool contains = tree.Contains(1);
        
        // Assert
        Assert.That(contains, Is.False);
    }

    [Test]
    public void ShouldDeleteKeyWithBothChildren()
    {
        // Arrange
        var tree = new BinarySearchTree<int, string>();
        Insert(tree);
        
        // Act
        tree.Remove(5);
        
        // Assert
        var root = GetRoot(tree);
        
        Assert.That(root, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(root!.Key, Is.EqualTo(10));
            Assert.That(root!.Left!.Key, Is.EqualTo(7));
            Assert.That(root!.Left!.Left!.Key, Is.EqualTo(2));
            Assert.That(root!.Right!.Key, Is.EqualTo(15));
        });
    }

    [Test]
    public void ShouldDeleteKeyWithLeftChild()
    {
        // Arrange
        var tree = new BinarySearchTree<int, string>();
        Insert(tree);
        
        // Act
        tree.Insert(12, "f");
        tree.Remove(15);
        
        // Assert
        var root = GetRoot(tree);
        
        Assert.That(root, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(root!.Key, Is.EqualTo(10));
            Assert.That(root!.Left!.Key, Is.EqualTo(5));
            Assert.That(root!.Left!.Left!.Key, Is.EqualTo(2));
            Assert.That(root!.Left!.Right!.Key, Is.EqualTo(7));
            Assert.That(root!.Right!.Key, Is.EqualTo(12));
        });
    }

    [Test]
    public void ShouldDeleteKeyWithRightChild()
    {
        // Arrange
        var tree = new BinarySearchTree<int, string>();
        Insert(tree);
        
        // Act
        tree.Insert(17, "f");
        tree.Remove(15);
        
        // Assert
        var root = GetRoot(tree);
        
        Assert.That(root, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(root!.Key, Is.EqualTo(10));
            Assert.That(root!.Left!.Key, Is.EqualTo(5));
            Assert.That(root!.Left!.Left!.Key, Is.EqualTo(2));
            Assert.That(root!.Left!.Right!.Key, Is.EqualTo(7));
            Assert.That(root!.Right!.Key, Is.EqualTo(17));
        });
    }

    [Test]
    public void ShouldDeleteRoot()
    {
        // Arrange
        var tree = new BinarySearchTree<int, string>();

        // Act
        tree.Insert(17, "f");
        tree.Remove(17);
        
        // Assert
        var root = GetRoot(tree);
        
        Assert.That(root, Is.Null);
    }

    [Test]
    public void ShouldDeleteNothingIfTreeIsEmpty()
    {
        // Arrange
        var tree = new BinarySearchTree<int, string>();

        // Act
        tree.Remove(17);
        
        // Assert
        var root = GetRoot(tree);
        
        Assert.That(root, Is.Null);
    }

    [Test]
    public void ShouldSetNewRoot()
    {
        // Arrange
        var tree = new BinarySearchTree<int, string>();

        // Act
        tree.Insert(15, "");
        tree.Insert(17, "");
        tree.Remove(15);
        
        // Assert
        var root = GetRoot(tree);
        
        Assert.That(root, Is.Not.Null);
        Assert.That(root!.Key, Is.EqualTo(17));
        Assert.That(root!.Left, Is.Null);
        Assert.That(root!.Right, Is.Null);
    }
    
    private static void Insert(BinarySearchTree<int, string> tree)
    {
        tree.Insert(10, "a");
        tree.Insert(5, "b");
        tree.Insert(2, "c");
        tree.Insert(7, "d");
        tree.Insert(15, "e");
    }
    
    private static BinarySearchTree<int, string>.Node? GetRoot(BinarySearchTree<int, string> tree)
    {
        return tree
            .GetType()
            .GetField("_root", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(tree) as BinarySearchTree<int, string>.Node;
    }
}
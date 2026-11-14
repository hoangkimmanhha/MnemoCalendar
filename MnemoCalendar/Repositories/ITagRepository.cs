using MnemoCalendar.Models;

namespace MnemoCalendar.Repositories;

public interface ITagRepository
{
    /// <summary>
    /// Create a tag in database.
    /// </summary>
    /// <returns>long tag_id</returns>
    public long CreateTag(Tag tag);

    /// <summary>
    /// Get details of a tag by its id.
    /// </summary>
    public Tag? GetTagByID(long id);

    public List<Tag>? GetAllTags();

    /// <summary>
    /// Permanently delete a tag, but does not delete its correspond events.
    /// </summary>
    public void DeleteTag(long id);
}
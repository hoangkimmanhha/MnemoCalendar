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
    public Tag? GetTagById(long id);

    public List<Tag>? GetAllTags();

    /// <summary>
    /// Update details of an Tag (only update Name and ColorHex).
    /// </summary>
    public void UpdateTag(Tag newTag);

    /// <summary>
    /// Permanently delete a tag, but does not delete its correspond events.
    /// </summary>
    public void DeleteTag(long id);
}
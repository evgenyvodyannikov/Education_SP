# Module 12 - Managing Taxonomy

## **Topics**:

1. [Understanding Taxonomy in SharePoint](#understanding-taxonomy-in-sharepoint)

## **Understanding Taxonomy in SharePoint**

In an information management system such as SharePoint, a robust taxonomy is essential. It enables content creators to **organize and categorize** their data, and it enables content consumers to locate data quickly through search or through navigation.

<p align="center">
  <img src="https://user-images.githubusercontent.com/66135471/217195344-feeb8918-8af0-4484-8abe-1541e685c866.png">
</p>

In SharePoint, taxonomy is closely tied to metadata, which you can define as data about data. 

For example, in the case of a Microsoft Word document, the data is the contents of the document, whereas the metadata is information about the document such as the author, the date it was created, and so on. Metadata enables users to find data by filtering, searching, or navigating. 

SharePoint provides three core building blocks that solution architects can use to design and implement a taxonomy:

- **Site columns**. To collect additional metadata about list items or files in SharePoint, you add a column to the list or library. Site columns are reusable columns that you can add to lists or libraries within the site.

- **Content types**. A content type defines and classifies a document or a list item according to the requirements of the organization. Content types can specify the site columns, workflows, policies, and document templates that should be associated with a particular item. For example, you might define a content type named Invoice. If a user uploads a document and specifies a content type of Invoice, SharePoint will prompt the user for any required field values, such as the invoice amount, the supplier, and the due date. 

- **Managed metadata term sets**. A term set is a **hierarchical** set of terms that is typically maintained by one or more term set administrators. Term sets provide a way of ensuring that users use a consistent nomenclature when adding metadata. You can create site columns based on a term set, so that users can only add terms from the term set when they populate certain fields. You can also use term sets to drive navigation on publishing sites.

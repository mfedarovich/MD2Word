# MD2Word
**MD2Word** is a markdown to Microsoft Word converter for .NET. It is based on [markdig](https://github.com/xoofx/markdig) markdown processor.
It create Word document, based on the predefined template. 
# Supported Features
* Headings
* Number lists
* Bullet lists
* Hyperlinks
* Emphasis
* Embedding images from local drive or web
* [PlantUML](http://plantuml.com/) - for UML diagram generation
* Code blocks (without syntax highlighting)

# Documentation
## Template Document
It can be any *.docx document, where all required styles are defined:
* Body text
* Code text
* Caption
* Code block
* Heading
* Number list
* Bullet list
* Hyperlink.

You need to ensure that all used styles are really saved in the word document. For that you need to simply write some dummy text, apply style on to it and save document. 
Then you can cleanup your template. 

Please see for reference [template example](template_example.docx).

## Configuration
All styles from template shall be mapped in the [appsettings.json](MD2Word.App\appsettings.json) like as follow:
```
{
"Styles": {
    "BodyText": "Normal",
    "CodeText": "Code Text",
    "Caption": "Subtitle",
    "CodeBlock": "Code",
    "Heading": "Heading {0}",
    "NumberList": "List Number",
    "BulletList": "List Bullet",
    "Hyperlink": "Hyperlink"
    }
}
```

## CLI 
| Short key | Long key | Description |
| --------- | ---------- | ----------- |
| -t        | --template | **Required**. Word document (*.docx), which is used as template for document generation|
| -m        | --markdown | **Required**. Input markdown file |
| -o        | --output   | _Optional_: Output file name. It shall be specified if output name shall differ from the name of markdown file|
|-d         | --dir      | _Optional_: Output directory, otherwise document will be generated nearby markdown file|


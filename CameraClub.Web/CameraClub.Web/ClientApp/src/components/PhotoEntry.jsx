import React, { Component } from 'react';
import { Row, Col, Label } from 'reactstrap';
import { InputWithChanges } from './InputWithChanges';

export class PhotoEntry extends Component {
    constructor(props) {
        super(props);

        this.uploadPhoto = this.uploadPhoto.bind(this);
    }

    uploadPhoto(fileData) {
        var fileInfo = new FormData();

        fileInfo.append(
            "file",
            fileData.target.files[0],
            fileData.target.files[0].name
        );

        this.props.uploadPhoto(this.props.id, fileInfo);
    }

    render() {
        return (
            <>
                <Row className="align-items-end">
                    <Col sm={3}>
                        <Label for="title">Title</Label>
                        <InputWithChanges type="text" name="title" placeholder="Title of photo" value={this.props.title} onChangeInput={(data) => { this.props.handleTitleChange(data.title, this.props.id); }} />
                    </Col>
                    <Col sm={2}>
                        <Label for="title">Category</Label>
                        <select className="form-control" value={this.props.categoryId} onChange={(e) => { this.props.handleCategoryChange(e.target.value, this.props.id); }}>
                            {this.props.categories.map(category =>
                                <option key={this.props.id + " " + category.id} value={category.id}>
                                    {category.name}
                                </option>
                            )}
                        </select>
                    </Col>
                    <Col sm={2}>
                        <button className={this.props.isDigital && this.props.fileName ? "btn btn-sm btn-outline-info form-control" : "invisible"}
                            onClick={(e) => { e.preventDefault(); this.props.viewPhoto(this.props.id); }}>View</button>
                    </Col>
                    <Col sm={3}>
                        <Label for="filePicker" className={this.props.isDigital && this.props.fileName ? "visible" : "invisible"}>Current File: {this.props.fileName}</Label>
                        <input id="filePicker" type="file" className={this.props.isDigital ? "visible" : "invisible"} onChange={(e) => { this.uploadPhoto(e); }} />
                    </Col>
                    <Col sm={2}>
                        <button className="btn btn-sm btn-outline-secondary form-control" onClick={(e) => { e.preventDefault(); this.props.removePhoto(this.props.id); }}>Remove</button>
                    </Col>
                </Row>
            </>
        );
    }
}
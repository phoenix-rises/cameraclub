import React, { Component } from 'react';
import { Row, Col, Label } from 'reactstrap';
import { InputWithChanges } from './InputWithChanges';

export class PhotoEntry extends Component {
    constructor(props) {
        super(props);

        this.uploadPhoto = this.uploadPhoto.bind(this);
    }

    uploadPhoto() {
        alert('should display file browing thing here');

        // TODO: display upload file browsing thing; determine if we are uploading now or on save.. think on save???

        this.props.uploadPhoto(this.props.id);
    }

    render() {
        return (
            <>
                <Row className="top-margin-spacing">
                    <Label for="title" sm={1}>Title</Label>
                    <Col sm={3}>
                        <InputWithChanges type="text" name="title" placeholder="Title of photo" value={this.props.title} onChangeInput={(data) => { this.props.handleTitleChange(data.title, this.props.id); }} />
                    </Col>
                    <Col sm={2}>
                        <select className="form-control" value={this.props.categoryId} onChange={(e) => { this.props.handleCategoryChange(e.target.value, this.props.id); }}>
                            {this.props.categories.map(category =>
                                <option key={this.props.id + " " + category.id} value={category.id}>
                                    {category.name}
                                </option>
                            )}
                        </select>
                    </Col>
                    <Col sm={2}>
                        <button className="btn btn-sm btn-outline-primary form-control" onClick={(e) => { e.preventDefault(); this.uploadPhoto(); }}>Upload</button>
                    </Col>
                    <Col sm={2}>
                        <button className="btn btn-sm btn-outline-info form-control" onClick={(e) => { e.preventDefault(); this.props.viewPhoto(this.props.id); }}>View</button>
                    </Col>
                    <Col sm={2}>
                        <button className="btn btn-sm btn-outline-secondary form-control" onClick={(e) => { e.preventDefault(); this.props.removePhoto(this.props.id); }}>Remove</button>
                    </Col>
                </Row>
            </>
        );
    }
}
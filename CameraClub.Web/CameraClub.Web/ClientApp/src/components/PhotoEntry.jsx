import React, { Component } from 'react';
import { Col, Label, Container } from 'reactstrap';
import { InputWithChanges } from './InputWithChanges';
import { PhotoModal } from './PhotoModal';

export class PhotoEntry extends Component {
    constructor(props) {
        super(props);

        this.state = { isModalVisible: false };

        this.uploadPhoto = this.uploadPhoto.bind(this);
        this.hideModal = this.hideModal.bind(this);
    }

    showModal = () => {
        this.setState({ isModalVisible: true });
    };

    hideModal = () => {
        this.setState({ isModalVisible: false });
    };

    uploadPhoto(fileData) {
        var fileInfo = new FormData();
        fileInfo.append("file", fileData.target.files[0]);

        this.props.uploadPhoto(this.props.id, fileData.target.files[0].name, fileInfo);
    }

    render() {
        return (
            <>
                <Container className="card mb-3">
                    <div className="card-body align-baseline pl-0 pr-0 form-row">
                        <Col sm={1}>
                            <Container>
                                <div className="form-row">
                                    <Col sm={2}>
                                        <button type="button" className="close" aria-label="Close" onClick={(e) => { e.preventDefault(); this.props.removePhoto(this.props.id); }}>
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </Col>
                                    <Col sm={10} className="text-right">
                                        <Label className="form-control-plaintext" for="title">Title</Label>
                                    </Col>
                                </div>
                            </Container>
                        </Col>
                        <Col sm={3}>
                            <InputWithChanges type="text" name="title" placeholder="Title of photo" value={this.props.title} onChangeInput={(data) => { this.props.handleTitleChange(data.title, this.props.id); }} />
                        </Col>
                        <Col sm={1} className="text-right">
                            <Label for="title" className="form-control-plaintext">Category</Label>
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
                            <button className={this.props.isDigital && this.props.fileName ? "btn btn-sm btn-link" : "invisible"}
                                onClick={(e) => { e.preventDefault(); this.showModal(); }}>{this.props.fileName}</button>
                        </Col>
                        <Col sm={3}>
                            <input id="filePicker" type="file" className={this.props.isDigital ? "visible" : "invisible"} onChange={(e) => { this.uploadPhoto(e); }} />
                        </Col>
                    </div>
                </Container>
                <PhotoModal handleClose={this.hideModal} photoData={this.props} show={this.state.isModalVisible} />
            </>
        );
    }
}
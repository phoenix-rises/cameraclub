import React, { Component } from 'react';
import { Row, Col } from 'reactstrap';
import { ClubApi } from '../ClubApi';
import { AddPhotographerEntry } from './AddPhotographerEntry';
import { PhotographerEntry } from './PhotographerEntry';

export class CompetitionPhotographers extends Component {
    clubApi;

    constructor(props) {
        super(props);

        this.clubApi = new ClubApi();

        var competitionId = props.match.params.competitionId;

        this.state = {
            competitionId: competitionId,
            competitionInfo: { "name": "", "hasDigital": false, "hasPrint": false },
            photographers: [],
            photos: [],
            categories: [],
            newPhotographerId: "0",
            newPhotoId: "0",
            error: false,
            loading: true,
            errorMessage: ""
        }

        this.loadEntriesState = this.loadEntriesState.bind(this);
        this.loadCategoryState = this.loadCategoryState.bind(this);
        this.showError = this.showError.bind(this);
        this.addPhoto = this.addPhoto.bind(this);
        this.removePhoto = this.removePhoto.bind(this);
        this.uploadPhoto = this.uploadPhoto.bind(this);
        this.addPhotographer = this.addPhotographer.bind(this);
        this.handleTitleChange = this.handleTitleChange.bind(this);
        this.handleCategoryChange = this.handleCategoryChange.bind(this);
        this.viewPhoto = this.viewPhoto.bind(this);
    }

    componentDidMount() {
        this.loadData();
    }

    loadData() {
        this.clubApi.load("GetCategories", this.showError, this.loadCategoryState);
    }

    loadCategoryState(categories) {
        this.setState({ "categories": categories });

        var test = {
            "competitionInfo": { "name": "Spring Extravaganza", "hasDigital": true, "hasPrint": true },
            "photographers":
                [
                    { "id": "1", "firstName": "Bob", "lastName": "Barker", "competitionNumber": "1234", "isDeleted": false },
                    { "id": "2", "firstName": "Jeff", "lastName": "McGee", "competitionNumber": "5555", "isDeleted": false }
                ],
            "photos":
                [
                    { "photographerId": "1", "id": 1, "title": "outdoor stuff", "categoryId": "1", "fileGuid": "blah1", "isDeleted": false },
                    { "photographerId": "1", "id": 2, "title": "people stuff", "categoryId": "2", "fileGuid": "blah2", "isDeleted": false },
                    { "photographerId": "2", "id": 3, "title": "Jeff's photo", "categoryId": "1", "fileGuid": "blah3", "isDeleted": false },
                    { "photographerId": "2", "id": 4, "title": "Jeff's second photo", "categoryId": "2", "fileGuid": "blah4", "isDeleted": false }
                ]
        }

        this.loadEntriesState(test); // TODO: replace this with api call below

        //this.clubApi.load("GetCompetitionEntries", this.showError, this.loadPhotographerState);
    }

    loadEntriesState(entryData) {
        this.setState({
            loading: false,
            error: false,
            errorMessage: null,
            competitionInfo: entryData.competitionInfo,
            photographers: entryData.photographers,
            photos: entryData.photos
        });
    }

    save() {
        // TODO: call save api
    }

    showError(error) {
        console.log(error);
        this.setState({
            loading: false,
            error: true,
            errorMessage: error
        });
    }

    addPhotographer(newPhotographer) {
        let photographers = [...this.state.photographers];

        newPhotographer.id = this.state.newPhotographerId - 1;
        photographers.push(newPhotographer);

        this.setState({ "photographers": photographers, newPhotographerId: newPhotographer.id });
    }

    addPhoto(photographerId) {
        var newId = this.state.newPhotoId - 1;
        var newPhoto = { "photographerId": photographerId, "id": newId, "title": "", "categoryId": "", "fileGuid": "", "isDeleted": false };
        let photos = [...this.state.photos, newPhoto];

        this.setState({ "photos": photos, newPhotoId: newId });
    }

    removePhoto(photoId) {
        this.updatePhotoState(photoId, (photoToUpdate) => { photoToUpdate.isDeleted = true });
    }

    uploadPhoto(photoId) {
        // TODO: update state with filename or something... depends on how we handle photo uploading...
    }

    viewPhoto(photoId) {
        // TODO: show photo, maybe in dialog or maybe in popup
    }

    handleTitleChange(newTitle, photoId) {
        this.updatePhotoState(photoId, (photoToUpdate) => { photoToUpdate.title = newTitle; });
    }

    handleCategoryChange(newCategory, photoId) {
        this.updatePhotoState(photoId, (photoToUpdate) => { photoToUpdate.categoryId = newCategory; });
    }

    updatePhotoState(photoId, updateMethod) {
        let photos = [...this.state.photos];

        var photoToUpdate = photos.find(p => p.id === photoId);
        updateMethod(photoToUpdate);

        this.setState({ "photos": photos });
    }

    renderCompetitionEntries() {
        return (
            <>
                <Row>
                    <Col>
                        <h1 className="page-title">Entries for {this.state.competitionInfo.name}</h1>
                    </Col>
                </Row>
                <AddPhotographerEntry addPhotographer={this.addPhotographer} />
                <Row>
                    {this.state.photographers.filter(p => !p.isDeleted).map(photographer =>
                        <PhotographerEntry key={photographer.id} photographer={photographer} photos={this.state.photos} categories={this.state.categories}
                            handleTitleChange={this.handleTitleChange} handleCategoryChange={this.handleCategoryChange}
                            addPhoto={this.addPhoto} uploadPhoto={this.uploadPhoto} removePhoto={this.removePhoto} viewPhoto={this.viewPhoto} />
                    )}
                </Row>
                <Row className="top-margin-spacing">
                    <Col className="modal-footer">
                        <a className="btn btn-secondary" href={"/"}>Cancel</a>
                        <button className="btn btn-primary" onClick={(e) => { e.preventDefault(); this.save(); }}>Save Changes</button>
                    </Col>
                </Row>
            </>
        );
    }

    render() {
        let contents = this.state.error
            ? <p>Error:  <span dangerouslySetInnerHTML={{ __html: this.state.errorMessage }}></span></p>
            : this.state.loading
                ? <p><em>Loading...</em></p>
                : this.renderCompetitionEntries();

        return (
            <div>
                {contents}
            </div>
        );
    }
}